USE Prueba;
GO
-- ============================================================
-- CONTROL DE CAJA
-- ============================================================
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
-- EGRESOS CAJA
-- ============================================================
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
-- RESUMEN DE CAJA ACTIVA
-- ============================================================
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