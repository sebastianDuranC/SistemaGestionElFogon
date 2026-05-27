-- ============================================================
-- SCRIPT DE CREACION DE PROCEDIMIENTOS ALMACENADOS
-- ============================================================

-- ============================================================
-- 1. MODULO: CLIENTES
-- ============================================================
GO
CREATE OR ALTER PROCEDURE sp_ListarClientes
AS
BEGIN
    SELECT Id, Nombre, Apellido, EsComerciante, NumeroLocal, Pasillo, Estado
    FROM Cliente
    WHERE Estado = 1;
END;
GO

CREATE OR ALTER PROCEDURE sp_ObtenerClientePorId
    @Id INT
AS
BEGIN
    SELECT Id, Nombre, Apellido, EsComerciante, NumeroLocal, Pasillo, Estado
    FROM Cliente
    WHERE Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE sp_CrearCliente
    @Nombre NVARCHAR(150),
    @Apellido NVARCHAR(150),
    @EsComerciante BIT,
    @NumeroLocal NVARCHAR(20) = NULL,
    @Pasillo NVARCHAR(50) = NULL
AS
BEGIN
    INSERT INTO Cliente (Nombre, Apellido, EsComerciante, NumeroLocal, Pasillo, Estado)
    VALUES (@Nombre, @Apellido, @EsComerciante, @NumeroLocal, @Pasillo, 1);
END;
GO

CREATE OR ALTER PROCEDURE sp_EditarCliente
    @Id INT,
    @Nombre NVARCHAR(150),
    @Apellido NVARCHAR(150),
    @EsComerciante BIT,
    @NumeroLocal NVARCHAR(20) = NULL,
    @Pasillo NVARCHAR(50) = NULL
AS
BEGIN
    UPDATE Cliente
    SET Nombre = @Nombre,
        Apellido = @Apellido,
        EsComerciante = @EsComerciante,
        NumeroLocal = @NumeroLocal,
        Pasillo = @Pasillo
    WHERE Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE sp_EliminarCliente
    @Id INT
AS
BEGIN
    UPDATE Cliente
    SET Estado = 0
    WHERE Id = @Id;
END;
GO

-- ============================================================
-- 2. MODULO: METODOS DE PAGO
-- ============================================================
GO
CREATE OR ALTER PROCEDURE sp_ListarMetodosPago
AS
BEGIN
    SELECT Id, Nombre, Estado
    FROM MetodoPago
    WHERE Estado = 1;
END;
GO

CREATE OR ALTER PROCEDURE sp_ObtenerMetodoPagoPorId
    @Id INT
AS
BEGIN
    SELECT Id, Nombre, Estado
    FROM MetodoPago
    WHERE Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE sp_CrearMetodoPago
    @Nombre NVARCHAR(100)
AS
BEGIN
    INSERT INTO MetodoPago (Nombre, Estado)
    VALUES (@Nombre, 1);
END;
GO

CREATE OR ALTER PROCEDURE sp_EditarMetodoPago
    @Id INT,
    @Nombre NVARCHAR(100)
AS
BEGIN
    UPDATE MetodoPago
    SET Nombre = @Nombre
    WHERE Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE sp_EliminarMetodoPago
    @Id INT
AS
BEGIN
    UPDATE MetodoPago
    SET Estado = 0
    WHERE Id = @Id;
END;
GO

-- ============================================================
-- 3. MODULO: MOVIMIENTO INVENTARIO
-- ============================================================
GO
CREATE OR ALTER PROCEDURE sp_ListarMovimientosInventario
AS
BEGIN
    SELECT m.Id, m.InsumoId, i.Nombre AS NombreInsumo, m.Fecha, m.TipoMovimiento, m.Cantidad, m.Observacion, m.UsuarioId, u.Nombre AS NombreUsuario, m.Estado
    FROM MovimientoInventario m
    INNER JOIN Insumo i ON m.InsumoId = i.Id
    INNER JOIN Usuario u ON m.UsuarioId = u.Id
    WHERE m.Estado = 1
    ORDER BY m.Fecha DESC;
END;
GO

CREATE OR ALTER PROCEDURE sp_RegistrarMovimientoInventario
    @InsumoId INT,
    @TipoMovimiento NVARCHAR(50),
    @Cantidad DECIMAL(10,2),
    @Observacion NVARCHAR(300),
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT OFF;
    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO MovimientoInventario (InsumoId, TipoMovimiento, Cantidad, Observacion, UsuarioId, Fecha, Estado)
        VALUES (@InsumoId, @TipoMovimiento, @Cantidad, @Observacion, @UsuarioId, GETDATE(), 1);

        UPDATE Insumo
        SET Stock = Stock + @Cantidad
        WHERE Id = @InsumoId;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- ============================================================
-- 4. MODULO: CIERRE INVENTARIO
-- ============================================================
GO
IF NOT EXISTS (SELECT * FROM sys.types WHERE name = 'CierreInventarioTipo' AND is_table_type = 1)
BEGIN
    CREATE TYPE CierreInventarioTipo AS TABLE (
        InsumoId INT,
        CantidadTeorica DECIMAL(10,2),
        CantidadReal DECIMAL(10,2),
        Diferencia DECIMAL(10,2),
        Observacion NVARCHAR(300)
    );
END;
GO

CREATE OR ALTER PROCEDURE sp_ListarCierresInventario
AS
BEGIN
    SELECT c.Id, c.FechaHora, c.CantidadTeorica, c.CantidadReal, c.Diferencia, c.Observacion, c.UsuarioId, u.Nombre AS NombreUsuario, c.InsumoId, i.Nombre AS NombreInsumo, c.Estado
    FROM CierreInventario c
    INNER JOIN Insumo i ON c.InsumoId = i.Id
    INNER JOIN Usuario u ON c.UsuarioId = u.Id
    WHERE c.Estado = 1
    ORDER BY c.FechaHora DESC;
END;
GO

CREATE OR ALTER PROCEDURE sp_RegistrarCierreInventario
    @Cierres CierreInventarioTipo READONLY,
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT OFF;
    BEGIN TRANSACTION;
    BEGIN TRY
        -- 1. Insertar en CierreInventario
        INSERT INTO CierreInventario (CantidadTeorica, CantidadReal, Diferencia, Observacion, FechaHora, Estado, UsuarioId, InsumoId)
        SELECT CantidadTeorica, CantidadReal, Diferencia, Observacion, GETDATE(), 1, @UsuarioId, InsumoId
        FROM @Cierres;

        -- 2. Actualizar stock del insumo a la cantidad real física contada
        UPDATE i
        SET i.Stock = c.CantidadReal
        FROM Insumo i
        INNER JOIN @Cierres c ON i.Id = c.InsumoId;

        -- 3. Generar movimiento de ajuste automático en la bitácora
        INSERT INTO MovimientoInventario (Fecha, TipoMovimiento, Cantidad, Observacion, Estado, InsumoId, UsuarioId)
        SELECT GETDATE(), 'Ajuste por cierre', Diferencia, ISNULL(Observacion, 'Ajuste automático de stock por cierre de inventario'), 1, InsumoId, @UsuarioId
        FROM @Cierres;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

-- ============================================================
-- 5. MODULO: CONTROL DE CAJA
-- ============================================================
GO
CREATE OR ALTER PROCEDURE sp_ObtenerEstadoCajaActual
AS
BEGIN
    SELECT Id, FechaHoraApertura, MontoApertura, FechaHoraCierre, MontoCierreEsperado, MontoCierreReal, Diferencial, Estado, UsuarioId, NegocioId
    FROM ControlCaja
    WHERE FechaHoraCierre IS NULL AND Estado = 1;
END;
GO

CREATE OR ALTER PROCEDURE sp_ListarControlCajaHistorico
AS
BEGIN
    SELECT cc.Id, cc.FechaHoraApertura, cc.MontoApertura, cc.FechaHoraCierre, cc.MontoCierreEsperado, cc.MontoCierreReal, cc.Diferencial, cc.Estado, cc.UsuarioId, u.Nombre AS NombreUsuario, cc.NegocioId
    FROM ControlCaja cc
    INNER JOIN Usuario u ON cc.UsuarioId = u.Id
    WHERE cc.FechaHoraCierre IS NOT NULL AND cc.Estado = 1
    ORDER BY cc.FechaHoraCierre DESC;
END;
GO

CREATE OR ALTER PROCEDURE sp_AbrirCaja
    @MontoApertura DECIMAL(10,2),
    @UsuarioId INT,
    @NegocioId INT
AS
BEGIN
    INSERT INTO ControlCaja (FechaHoraApertura, MontoApertura, Estado, UsuarioId, NegocioId)
    VALUES (GETDATE(), @MontoApertura, 1, @UsuarioId, @NegocioId);
END;
GO

CREATE OR ALTER PROCEDURE sp_CerrarCaja
    @Id INT,
    @MontoCierreEsperado DECIMAL(10,2),
    @MontoCierreReal DECIMAL(10,2),
    @Diferencial DECIMAL(10,2)
AS
BEGIN
    UPDATE ControlCaja
    SET FechaHoraCierre = GETDATE(),
        MontoCierreEsperado = @MontoCierreEsperado,
        MontoCierreReal = @MontoCierreReal,
        Diferencial = @Diferencial
    WHERE Id = @Id;
END;
GO

-- ============================================================
-- 6. MODULO: EGRESOS CAJA
-- ============================================================
GO
CREATE OR ALTER PROCEDURE sp_ListarEgresosCaja
    @ControlCajaId INT = NULL
AS
BEGIN
    SELECT ec.Id, ec.Fecha, ec.Motivo, ec.Monto, ec.Estado, ec.ControlCajaId, ec.UsuarioId, u.Nombre AS NombreUsuario
    FROM EgresosCaja ec
    INNER JOIN Usuario u ON ec.UsuarioId = u.Id
    WHERE ec.Estado = 1
      AND (@ControlCajaId IS NULL OR ec.ControlCajaId = @ControlCajaId)
    ORDER BY ec.Fecha DESC;
END;
GO

CREATE OR ALTER PROCEDURE sp_RegistrarEgresoCaja
    @Motivo NVARCHAR(150),
    @Monto DECIMAL(10,2),
    @ControlCajaId INT,
    @UsuarioId INT
AS
BEGIN
    INSERT INTO EgresosCaja (Fecha, Motivo, Monto, Estado, ControlCajaId, UsuarioId)
    VALUES (GETDATE(), @Motivo, @Monto, 1, @ControlCajaId, @UsuarioId);
END;
GO

-- ============================================================
-- 7. PROCEDIMIENTO AUXILIAR: RESUMEN DE CAJA ACTIVA
-- ============================================================
GO
CREATE OR ALTER PROCEDURE sp_ObtenerResumenCaja
    @ControlCajaId INT
AS
BEGIN
    DECLARE @MontoApertura DECIMAL(10,2);
    DECLARE @FechaApertura DATETIME;
    DECLARE @FechaCierre DATETIME;

    SELECT @MontoApertura = MontoApertura, 
           @FechaApertura = FechaHoraApertura,
           @FechaCierre = ISNULL(FechaHoraCierre, GETDATE())
    FROM ControlCaja
    WHERE Id = @ControlCajaId;

    -- Calcular Ventas en Efectivo del turno
    DECLARE @VentasEfectivo DECIMAL(10,2);
    SELECT @VentasEfectivo = ISNULL(SUM(dp.Monto), 0)
    FROM DetallePago dp
    INNER JOIN Venta v ON dp.VentaId = v.Id
    INNER JOIN MetodoPago mp ON dp.MetodoPagoId = mp.Id
    WHERE dp.Estado = 1 
      AND v.Estado = 1 
      AND mp.Nombre = 'Efectivo'
      AND v.Fecha >= @FechaApertura 
      AND v.Fecha <= @FechaCierre;

    -- Calcular Egresos del turno
    DECLARE @TotalEgresos DECIMAL(10,2);
    SELECT @TotalEgresos = ISNULL(SUM(Monto), 0)
    FROM EgresosCaja
    WHERE ControlCajaId = @ControlCajaId AND Estado = 1;

    -- Retornar resultados
    SELECT @MontoApertura AS MontoApertura,
           @VentasEfectivo AS VentasEfectivo,
           @TotalEgresos AS TotalEgresos,
           (@MontoApertura + @VentasEfectivo - @TotalEgresos) AS MontoCierreEsperado;
END;
GO
