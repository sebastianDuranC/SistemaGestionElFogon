USE Prueba;
GO

-- ==========================================
-- PROCEDIMIENTOS ALMACENADOS: PROVEEDOR
-- ==========================================
CREATE OR ALTER PROCEDURE sp_ListarProveedores
AS BEGIN
    SELECT Id, Nombre, Apellido, Contacto, Estado 
    FROM Proveedor
    WHERE Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_ObtenerProveedorPorId
    @Id INT
AS BEGIN
    SELECT Id, Nombre, Apellido, Contacto, Estado 
    FROM Proveedor
    WHERE Id = @Id AND Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_CrearProveedor
    @Nombre NVARCHAR(150),
    @Apellido NVARCHAR(150),
    @Contacto NVARCHAR(100)
AS BEGIN
    INSERT INTO Proveedor (Nombre, Apellido, Contacto, Estado)
    VALUES (@Nombre, @Apellido, @Contacto, 1);
    SELECT SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_EditarProveedor
    @Id INT,
    @Nombre NVARCHAR(150),
    @Apellido NVARCHAR(150),
    @Contacto NVARCHAR(100)
AS BEGIN
    UPDATE Proveedor
    SET Nombre = @Nombre, Apellido = @Apellido, Contacto = @Contacto
    WHERE Id = @Id AND Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_EliminarProveedor
    @Id INT
AS BEGIN
    UPDATE Proveedor
    SET Estado = 0
    WHERE Id = @Id AND Estado = 1;
END
GO

-- ==========================================
-- PROCEDIMIENTOS ALMACENADOS: PRODUCTOCATEGORIA
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
-- PROCEDIMIENTOS ALMACENADOS: INSUMOCATEGORIA
-- ==========================================
CREATE OR ALTER PROCEDURE sp_ListarInsumoCategorias
AS BEGIN
    SELECT Id, Nombre, Estado 
    FROM InsumoCategoria
    WHERE Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_ObtenerInsumoCategoriaPorId
    @Id INT
AS BEGIN
    SELECT Id, Nombre, Estado 
    FROM InsumoCategoria
    WHERE Id = @Id AND Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_CrearInsumoCategoria
    @Nombre NVARCHAR(100)
AS BEGIN
    INSERT INTO InsumoCategoria (Nombre, Estado)
    VALUES (@Nombre, 1);
    SELECT SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_EditarInsumoCategoria
    @Id INT,
    @Nombre NVARCHAR(100)
AS BEGIN
    UPDATE InsumoCategoria
    SET Nombre = @Nombre
    WHERE Id = @Id AND Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_EliminarInsumoCategoria
    @Id INT
AS BEGIN
    UPDATE InsumoCategoria
    SET Estado = 0
    WHERE Id = @Id AND Estado = 1;
END
GO

-- ==========================================
-- PROCEDIMIENTOS ALMACENADOS: UNIDADESMEDIDA
-- ==========================================
CREATE OR ALTER PROCEDURE sp_ListarUnidadesMedida
AS BEGIN
    SELECT Id, Nombre, Abreviatura, Estado 
    FROM UnidadesMedida
    WHERE Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_ObtenerUnidadMedidaPorId
    @Id INT
AS BEGIN
    SELECT Id, Nombre, Abreviatura, Estado 
    FROM UnidadesMedida
    WHERE Id = @Id AND Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_CrearUnidadMedida
    @Nombre NVARCHAR(50),
    @Abreviatura NVARCHAR(50)
AS BEGIN
    INSERT INTO UnidadesMedida (Nombre, Abreviatura, Estado)
    VALUES (@Nombre, @Abreviatura, 1);
    SELECT SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_EditarUnidadMedida
    @Id INT,
    @Nombre NVARCHAR(50),
    @Abreviatura NVARCHAR(50)
AS BEGIN
    UPDATE UnidadesMedida
    SET Nombre = @Nombre, Abreviatura = @Abreviatura
    WHERE Id = @Id AND Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_EliminarUnidadMedida
    @Id INT
AS BEGIN
    UPDATE UnidadesMedida
    SET Estado = 0
    WHERE Id = @Id AND Estado = 1;
END
GO

-- ==========================================
-- PROCEDIMIENTOS ALMACENADOS: INSUMO
-- ==========================================
CREATE OR ALTER PROCEDURE sp_ListarInsumos
AS BEGIN
    SELECT i.Id, i.Nombre, i.Costo, i.Stock, i.StockMinimo, i.FotoUrl, i.Estado,
           i.InsumoCategoriaId, ic.Nombre AS NombreCategoria,
           i.ProveedorId, (p.Nombre + ' ' + p.Apellido) AS NombreProveedor,
           i.UnidadesMedidaId, um.Nombre AS NombreMedidas
    FROM Insumo i
    INNER JOIN InsumoCategoria ic ON i.InsumoCategoriaId = ic.Id
    INNER JOIN Proveedor p ON i.ProveedorId = p.Id
    INNER JOIN UnidadesMedida um ON i.UnidadesMedidaId = um.Id
    WHERE i.Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_ObtenerInsumoPorId
    @Id INT
AS BEGIN
    SELECT i.Id, i.Nombre, i.Costo, i.Stock, i.StockMinimo, i.FotoUrl, i.Estado,
           i.InsumoCategoriaId, ic.Nombre AS NombreCategoria,
           i.ProveedorId, (p.Nombre + ' ' + p.Apellido) AS NombreProveedor,
           i.UnidadesMedidaId, um.Nombre AS NombreMedidas
    FROM Insumo i
    INNER JOIN InsumoCategoria ic ON i.InsumoCategoriaId = ic.Id
    INNER JOIN Proveedor p ON i.ProveedorId = p.Id
    INNER JOIN UnidadesMedida um ON i.UnidadesMedidaId = um.Id
    WHERE i.Id = @Id AND i.Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_CrearInsumo
    @Nombre NVARCHAR(150),
    @Costo DECIMAL(10,2),
    @Stock DECIMAL(10,2),
    @StockMinimo DECIMAL(10,2),
    @FotoUrl NVARCHAR(MAX),
    @InsumoCategoriaId INT,
    @ProveedorId INT,
    @UnidadesMedidaId INT
AS BEGIN
    INSERT INTO Insumo (Nombre, Costo, Stock, StockMinimo, FotoUrl, InsumoCategoriaId, ProveedorId, UnidadesMedidaId, Estado)
    VALUES (@Nombre, @Costo, @Stock, @StockMinimo, @FotoUrl, @InsumoCategoriaId, @ProveedorId, @UnidadesMedidaId, 1);
    SELECT SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE sp_EditarInsumo
    @Id INT,
    @Nombre NVARCHAR(150),
    @Costo DECIMAL(10,2),
    @Stock DECIMAL(10,2),
    @StockMinimo DECIMAL(10,2),
    @FotoUrl NVARCHAR(MAX),
    @InsumoCategoriaId INT,
    @ProveedorId INT,
    @UnidadesMedidaId INT
AS BEGIN
    UPDATE Insumo
    SET Nombre = @Nombre, Costo = @Costo, Stock = @Stock, StockMinimo = @StockMinimo, FotoUrl = @FotoUrl,
        InsumoCategoriaId = @InsumoCategoriaId, ProveedorId = @ProveedorId, UnidadesMedidaId = @UnidadesMedidaId
    WHERE Id = @Id AND Estado = 1;
END
GO

CREATE OR ALTER PROCEDURE sp_EliminarInsumo
    @Id INT
AS BEGIN
    UPDATE Insumo
    SET Estado = 0
    WHERE Id = @Id AND Estado = 1;
END
GO

-- ==========================================
-- PROCEDIMIENTOS ALMACENADOS: PRODUCTO
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

-- 1. Crear el tipo de tabla si no existe
IF NOT EXISTS (SELECT * FROM sys.types WHERE name = 'RecetaInsumoTipo' AND is_table_type = 1)
BEGIN
    CREATE TYPE RecetaInsumoTipo AS TABLE (
        InsumoId INT,
        Cantidad DECIMAL(10,2),
        Tipo NVARCHAR(20)
    );
END
GO

-- 2. Modificar sp_CrearProducto para recibir el TVP
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

-- 3. Modificar sp_EditarProducto para recibir el TVP
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
END
GO

-- ==========================================
-- PROCEDIMIENTOS ALMACENADOS: PRODUCTOINSUMO
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