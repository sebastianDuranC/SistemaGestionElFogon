USE Prueba;
GO
-- ============================================================
-- CLIENTES
-- ============================================================
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
-- METODOS DE PAGO
-- ============================================================
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

-- ==========================================
-- PRODUCTOCATEGORIA
-- ==========================================
CREATE OR ALTER PROCEDURE sp_ListarProductoCategorias
AS BEGIN
    SELECT Id, Nombre, Estado 
    FROM ProductoCategoria
    WHERE Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_ObtenerProductoCategoriaPorId
    @Id INT
AS BEGIN
    SELECT Id, Nombre, Estado 
    FROM ProductoCategoria
    WHERE Id = @Id AND Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_CrearProductoCategoria
    @Nombre NVARCHAR(100)
AS BEGIN
    INSERT INTO ProductoCategoria (Nombre, Estado)
    VALUES (@Nombre, 1);
    SELECT SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_EditarProductoCategoria
    @Id INT,
    @Nombre NVARCHAR(100)
AS BEGIN
    UPDATE ProductoCategoria
    SET Nombre = @Nombre
    WHERE Id = @Id AND Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_EliminarProductoCategoria
    @Id INT
AS BEGIN
    UPDATE ProductoCategoria
    SET Estado = 0
    WHERE Id = @Id AND Estado = 1;
END
GO

-- ==========================================
-- PRODUCTO
-- ==========================================
CREATE OR ALTER PROCEDURE sp_ListarProductos
AS BEGIN
    SELECT p.Id, p.Nombre, p.Precio, p.FotoUrl, p.Estado, p.ProductoCategoriaId, pc.Nombre AS NombreCategoria
    FROM Producto p
    INNER JOIN ProductoCategoria pc ON p.ProductoCategoriaId = pc.Id
    WHERE p.Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_ObtenerProductoPorId
    @Id INT
AS BEGIN
    SELECT p.Id, p.Nombre, p.Precio, p.FotoUrl, p.Estado, p.ProductoCategoriaId, pc.Nombre AS NombreCategoria
    FROM Producto p
    INNER JOIN ProductoCategoria pc ON p.ProductoCategoriaId = pc.Id
    WHERE p.Id = @Id AND p.Estado = 1;
END
GO

CREATE TYPE RecetaInsumoTipo AS TABLE (
        InsumoId INT,
        Cantidad DECIMAL(10,2),
        Tipo NVARCHAR(20)
    );
GO

CREATE OR ALTER PROCEDURE sp_CrearProducto
    @Nombre NVARCHAR(150),
    @Precio DECIMAL(10,2),
    @FotoUrl NVARCHAR(300),
    @ProductoCategoriaId INT,
    @Receta RecetaInsumoTipo READONLY
AS BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO Producto (Nombre, Precio, FotoUrl, ProductoCategoriaId, Estado)
        VALUES (@Nombre, @Precio, @FotoUrl, @ProductoCategoriaId, 1);

        DECLARE @ProductoId INT = SCOPE_IDENTITY();

        INSERT INTO ProductoInsumo (ProductoId, InsumoId, Cantidad, Tipo, Estado)
        SELECT @ProductoId, InsumoId, Cantidad, Tipo, 1
        FROM @Receta;

        COMMIT TRANSACTION;
        SELECT @ProductoId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_EditarProducto
    @Id INT,
    @Nombre NVARCHAR(150),
    @Precio DECIMAL(10,2),
    @FotoUrl NVARCHAR(300),
    @ProductoCategoriaId INT,
    @Receta RecetaInsumoTipo READONLY
AS BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        UPDATE Producto
        SET Nombre = @Nombre, Precio = @Precio, FotoUrl = @FotoUrl, ProductoCategoriaId = @ProductoCategoriaId
        WHERE Id = @Id AND Estado = 1;

        DELETE FROM ProductoInsumo WHERE ProductoId = @Id;

        INSERT INTO ProductoInsumo (ProductoId, InsumoId, Cantidad, Tipo, Estado)
        SELECT @Id, InsumoId, Cantidad, Tipo, 1
        FROM @Receta;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

CREATE OR ALTER PROCEDURE sp_EliminarProducto
    @Id INT
AS BEGIN
    UPDATE Producto
    SET Estado = 0
    WHERE Id = @Id AND Estado = 1;

    DELETE FROM ProductoInsumo WHERE ProductoId = @Id;
END
GO

-- ==========================================
-- PRODUCTOINSUMO
-- ==========================================
CREATE OR ALTER PROCEDURE sp_ObtenerInsumosPorProducto
    @ProductoId INT
AS BEGIN
    SELECT pi.InsumoId, i.Nombre AS InsumoNombre, pi.Cantidad, pi.Tipo
    FROM ProductoInsumo pi
    INNER JOIN Insumo i ON pi.InsumoId = i.Id
    WHERE pi.ProductoId = @ProductoId AND pi.Estado = 1 AND i.Estado = 1;
END
GO

-- ==========================================
-- TIPOS DE TABLAS PARA VENTA
-- ==========================================
IF NOT EXISTS (SELECT 1 FROM sys.types WHERE name = 'DetalleVentaTipo' AND is_user_defined = 1)
BEGIN
    CREATE TYPE DetalleVentaTipo AS TABLE (
        ProductoId INT,
        Cantidad INT,
        PrecioUnitario DECIMAL(10,2),
        SubTotal DECIMAL(10,2)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.types WHERE name = 'DetallePagoTipo' AND is_user_defined = 1)
BEGIN
    CREATE TYPE DetallePagoTipo AS TABLE (
        MetodoPagoId INT,
        Monto DECIMAL(10,2)
    );
END
GO

-- ==========================================
-- VENTAS
-- ==========================================
CREATE OR ALTER PROCEDURE sp_ListarVentas
AS
BEGIN
    SELECT v.Id, v.Fecha, v.Total, v.EnLocal, CASE WHEN v.EnLocal = 1 THEN 'En Local' ELSE 'Para Llevar' END AS TipoVenta,
           v.PlatoPrestado, v.MontoRecibido, v.CambioDevuelto, v.Estado, 
           v.ClienteId, (c.Nombre + ' ' + ISNULL(c.Apellido, '')) AS Cliente,
           v.UsuarioId, u.Nombre AS Vendedor
    FROM Venta v
    LEFT JOIN Cliente c ON v.ClienteId = c.Id
    INNER JOIN Usuario u ON v.UsuarioId = u.Id
    WHERE v.Estado = 1
    ORDER BY v.Fecha DESC;
END;
GO

CREATE OR ALTER PROCEDURE sp_ObtenerVentaPorId
    @Id INT
AS
BEGIN
    SELECT v.Id, v.Fecha, v.Total, v.EnLocal, CASE WHEN v.EnLocal = 1 THEN 'En Local' ELSE 'Para Llevar' END AS TipoVenta,
           v.PlatoPrestado, v.MontoRecibido, v.CambioDevuelto, v.Estado, 
           v.ClienteId, (c.Nombre + ' ' + ISNULL(c.Apellido, '')) AS Cliente,
           v.UsuarioId, u.Nombre AS Vendedor
    FROM Venta v
    LEFT JOIN Cliente c ON v.ClienteId = c.Id
    INNER JOIN Usuario u ON v.UsuarioId = u.Id
    WHERE v.Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE sp_ObtenerDetallesVenta
    @VentaId INT
AS
BEGIN
    SELECT dv.Id, dv.VentaId, dv.ProductoId, p.Nombre AS ProductoNombre, 
           dv.PrecioUnitario, dv.Cantidad, dv.SubTotal, dv.Estado
    FROM DetalleVenta dv
    INNER JOIN Producto p ON dv.ProductoId = p.Id
    WHERE dv.VentaId = @VentaId AND dv.Estado = 1;
END;
GO

CREATE OR ALTER PROCEDURE sp_ObtenerDetallesPago
    @VentaId INT
AS
BEGIN
    SELECT dp.Id, dp.VentaId, dp.MetodoPagoId, mp.Nombre AS MetodoPagoNombre, 
           dp.Monto, dp.Estado
    FROM DetallePago dp
    INNER JOIN MetodoPago mp ON dp.MetodoPagoId = mp.Id
    WHERE dp.VentaId = @VentaId AND dp.Estado = 1;
END;
GO

CREATE OR ALTER PROCEDURE sp_DevolverPlatos
    @Id INT
AS
BEGIN
    UPDATE Venta
    SET PlatoPrestado = 0
    WHERE Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE sp_CrearVenta
    @ClienteId INT = NULL,
    @UsuarioId INT,
    @Total DECIMAL(10,2),
    @EnLocal BIT,
    @PlatoPrestado BIT = NULL,
    @MontoRecibido DECIMAL(10,2),
    @CambioDevuelto DECIMAL(10,2),
    @Detalles DetalleVentaTipo READONLY,
    @Pagos DetallePagoTipo READONLY
AS
BEGIN
    SET NOCOUNT OFF;
    BEGIN TRANSACTION;
    BEGIN TRY
        -- 1. Registrar Venta cabecera
        INSERT INTO Venta (Fecha, Total, EnLocal, PlatoPrestado, MontoRecibido, CambioDevuelto, Estado, ClienteId, UsuarioId)
        VALUES (GETDATE(), @Total, @EnLocal, @PlatoPrestado, @MontoRecibido, @CambioDevuelto, 1, @ClienteId, @UsuarioId);

        DECLARE @VentaId INT = SCOPE_IDENTITY();

        -- 2. Registrar Detalle de Venta
        INSERT INTO DetalleVenta (Cantidad, PrecioUnitario, SubTotal, Estado, VentaId, ProductoId)
        SELECT Cantidad, PrecioUnitario, SubTotal, 1, @VentaId, ProductoId
        FROM @Detalles;

        -- 3. Registrar Detalle de Pago
        INSERT INTO DetallePago (Monto, Estado, VentaId, MetodoPagoId)
        SELECT Monto, 1, @VentaId, MetodoPagoId
        FROM @Pagos;

        -- 4. Descontar stock de insumos relacionados al producto vendido (según la receta en ProductoInsumo)
        UPDATE i
        SET i.Stock = i.Stock - (dv.Cantidad * pi.Cantidad)
        FROM Insumo i
        INNER JOIN ProductoInsumo pi ON i.Id = pi.InsumoId
        INNER JOIN @Detalles dv ON pi.ProductoId = dv.ProductoId
        WHERE pi.Estado = 1;

        -- 5. Registrar el movimiento en la bitácora de inventario como Salida
        INSERT INTO MovimientoInventario (Fecha, TipoMovimiento, Cantidad, Observacion, UsuarioId, Estado, InsumoId)
        SELECT GETDATE(), 'Salida', dv.Cantidad * pi.Cantidad, 'Salida por Venta #' + CAST(@VentaId AS NVARCHAR), @UsuarioId, 1, pi.InsumoId
        FROM @Detalles dv
        INNER JOIN ProductoInsumo pi ON dv.ProductoId = pi.ProductoId
        WHERE pi.Estado = 1;

        COMMIT TRANSACTION;
        SELECT @VentaId;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE sp_AnularVenta
    @Id INT,
    @UsuarioId INT
AS
BEGIN
    SET NOCOUNT OFF;
    BEGIN TRANSACTION;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Venta WHERE Id = @Id AND Estado = 1)
        BEGIN
            -- 1. Anular registros cambiándole el estado
            UPDATE Venta SET Estado = 0 WHERE Id = @Id;
            UPDATE DetalleVenta SET Estado = 0 WHERE VentaId = @Id;
            UPDATE DetallePago SET Estado = 0 WHERE VentaId = @Id;

            -- 2. Devolver el stock a los insumos correspondientes
            UPDATE i
            SET i.Stock = i.Stock + (dv.Cantidad * pi.Cantidad)
            FROM Insumo i
            INNER JOIN ProductoInsumo pi ON i.Id = pi.InsumoId
            INNER JOIN DetalleVenta dv ON pi.ProductoId = dv.ProductoId
            WHERE dv.VentaId = @Id AND pi.Estado = 1;

            -- 3. Registrar el movimiento de entrada por anulación
            INSERT INTO MovimientoInventario (Fecha, TipoMovimiento, Cantidad, Observacion, UsuarioId, Estado, InsumoId)
            SELECT GETDATE(), 'Entrada', dv.Cantidad * pi.Cantidad, 'Anulación de la venta #' + CAST(@Id AS NVARCHAR), @UsuarioId, 1, pi.InsumoId
            FROM DetalleVenta dv
            INNER JOIN ProductoInsumo pi ON dv.ProductoId = pi.ProductoId
            WHERE dv.VentaId = @Id AND pi.Estado = 1;
        END

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH;
END;
GO