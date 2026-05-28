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