USE Prueba;
GO
------------------------------------------
--- NEGOCIO
------------------------------------------
CREATE PROCEDURE sp_ListarNegocio
AS BEGIN
	SELECT Id, Nombre, Direccion, LogoUrl, Estado FROM Negocio
	WHERE Estado = 1;
END
GO

CREATE PROCEDURE sp_ObtenerNegocioPorId
	@Id INT
AS BEGIN
	SELECT Id, Nombre, Direccion, LogoUrl, Estado FROM Negocio
	WHERE Id = @Id AND Estado = 1;
END
GO

CREATE PROCEDURE sp_EditarNegocio
	@Id INT,
	@Nombre NVARCHAR(100),
	@Direccion NVARCHAR(200),
	@LogoUrl NVARCHAR(200)
AS BEGIN
	UPDATE Negocio
	SET Nombre = @Nombre, Direccion = @Direccion, LogoUrl = @LogoUrl
	WHERE Id = @Id AND Estado = 1;
END
GO
------------------------------------------
--- USUARIOS
------------------------------------------
CREATE PROCEDURE sp_ListarUsuarios
AS BEGIN
	SELECT U.Id, U.Nombre, U.Contra, U.RolId, R.Nombre AS NombreRol, U.NegocioId, N.Nombre AS NombreNegocio, U.Estado FROM Usuario AS U
	INNER JOIN Rol AS R ON U.RolId = R.Id
	INNER JOIN Negocio AS N ON U.NegocioId = N.Id
	WHERE U.Estado = 1;
END
GO

CREATE PROCEDURE sp_ObtenerUsuarioPorNombre
	@Nombre NVARCHAR(100)
AS BEGIN
	SELECT U.Id, U.Nombre, U.Contra, U.RolId, R.Nombre AS NombreRol, U.NegocioId, N.Nombre AS NombreNegocio, U.Estado FROM Usuario AS U
	INNER JOIN Rol AS R ON U.RolId = R.Id
	INNER JOIN Negocio AS N ON U.NegocioId = N.Id
	WHERE U.Nombre = @Nombre AND U.Estado = 1;
END
GO

CREATE PROCEDURE sp_ObtenerUsuarioPorId
	@Id INT
AS BEGIN
	SELECT U.Id, U.Nombre, U.Contra, U.RolId, R.Nombre AS NombreRol, U.NegocioId, N.Nombre AS NombreNegocio, U.Estado FROM Usuario AS U
	INNER JOIN Rol AS R ON U.RolId = R.Id
	INNER JOIN Negocio AS N ON U.NegocioId = N.Id
	WHERE U.Id = @Id AND U.Estado = 1;
END
GO

CREATE PROCEDURE sp_CrearUsuario
	@Nombre NVARCHAR(100),
	@Contra NVARCHAR(300),
	@RolId INT,
	@NegocioId INT,
	@Estado BIT
AS BEGIN
	INSERT INTO Usuario (Nombre, Contra, RolId, NegocioId, Estado)
	VALUES (@Nombre, @Contra, @RolId, @NegocioId, @Estado);
END
GO

CREATE PROCEDURE sp_EditarUsuario
	@Id INT,
	@Nombre NVARCHAR(100),
	@Contra NVARCHAR(300),
	@RolId INT,
	@NegocioId INT,
	@Estado BIT
AS BEGIN
	UPDATE Usuario
	SET Nombre = @Nombre, Contra = @Contra, RolId = @RolId, NegocioId = @NegocioId, Estado = @Estado
	WHERE Id = @Id AND Estado = 1;
END
GO

CREATE PROCEDURE sp_EliminarUsuario
	@Id INT
AS BEGIN
	UPDATE Usuario
	SET Estado = 0
	WHERE Id = @Id AND Estado = 1;
END
GO
---------------------------------------------
---- ROLES
---------------------------------------------
CREATE PROCEDURE sp_ListarRoles
AS
BEGIN
	SELECT Id, Nombre, Estado FROM Rol
	WHERE Estado = 1;
END
GO

CREATE PROCEDURE sp_ObtenerRolPorId
	@Id INT
AS
BEGIN
	SELECT Id, Nombre, Estado FROM Rol
	WHERE Id = @Id AND Estado = 1;
END
GO

CREATE PROCEDURE sp_CrearRol
	@Nombre NVARCHAR(100),
	@Estado BIT
AS BEGIN
	INSERT INTO Rol (Nombre, Estado)
	VALUES (@Nombre, @Estado);
	SELECT SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE sp_EditarRol
	@Id INT,
	@Nombre NVARCHAR(100),
	@Estado BIT
AS BEGIN
	UPDATE Rol
	SET Nombre = @Nombre, Estado = @Estado
	WHERE Id = @Id;
END
GO

CREATE PROCEDURE sp_EliminarRol
	@Id INT
AS BEGIN
	UPDATE Rol
	SET Estado = 0
	WHERE Id = @Id;
END
GO
---------------------------------------------
---- PERMISOS
---------------------------------------------
CREATE PROCEDURE sp_ListarPermisos
AS
BEGIN
	SELECT Id, FormNombre, FormRuta, Modulo, Estado FROM Permisos
	WHERE Estado = 1;
END
GO
---------------------------------------------
---- ROLPERMISOS
---------------------------------------------
CREATE PROCEDURE sp_ObtenerRutasPermitidasPorRol
    @RolId INT
AS
BEGIN --/Dashboard, Venta/Index, Cliente/Index, Usuario/Edit, etc
    SELECT p.FormRuta FROM RolPermisos rp
    INNER JOIN Permisos p ON rp.PermisosId = p.Id
    WHERE rp.RolId = @RolId AND rp.Estado = 1 AND p.Estado = 1;
END
GO

CREATE PROCEDURE sp_ObtenerPermisosPorRol
    @RolId INT
AS BEGIN
    SELECT PermisosId FROM RolPermisos WHERE RolId = @RolId AND Estado = 1;
END
GO

CREATE PROCEDURE sp_ActualizarRolPermisos
    @RolId INT,
    @PermisosIds NVARCHAR(MAX)
AS BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        DELETE FROM RolPermisos WHERE RolId = @RolId;
        
        IF @PermisosIds IS NOT NULL AND @PermisosIds <> ''
        BEGIN
            INSERT INTO RolPermisos (RolId, PermisosId, Estado)
            SELECT @RolId, CAST(value AS INT), 1
            FROM STRING_SPLIT(@PermisosIds, ',');
        END
        
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO