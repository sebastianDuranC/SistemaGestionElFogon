USE Prueba;
GO

INSERT INTO Negocio (Nombre, Direccion, LogoUrl)
VALUES ('El Fogón', 'Av. Principal 123, Ciudad', 'C:\\users\\sd858\\Downloads\\logo.png');
GO
-- ================================================================
-- ROLES
-- ================================================================
INSERT INTO Rol (Nombre)
VALUES ('Administrador'),
       ('Cajero');
GO

-- ================================================================
-- USUARIOS DE PRUEBA (contraseña hasheada con BCrypt)
-- ================================================================
INSERT INTO Usuario (Nombre, Contra, NegocioId, RolId)
VALUES ('Romel', '$2a$11$Zhs4wRW6CUXxOUOvIJczNuWcKIMPPZNR.XudrE5VfWKJ1FD.Z0ECy', 1, 1),
       ('Maria',  '$2a$11$Zhs4wRW6CUXxOUOvIJczNuWcKIMPPZNR.XudrE5VfWKJ1FD.Z0ECy', 1, 2);
GO

--DELETE FROM Permisos;
--DELETE FROM RolPermisos;
-- ================================================================
-- PERMISOS (rutas de Razor Pages, NO Web Forms)
-- ================================================================
INSERT INTO Permisos (FormRuta, FormNombre, Modulo)
VALUES
-- ============================================================
-- Dashboard
-- ============================================================
('/Index',                          'Dashboard',         'General'),
-- ============================================================
-- Módulo: Acceso y Administración
-- ============================================================
('/Negocio/Edit',                   'Negocio',           'Administración'),
('/Usuario/Index',                  'Usuarios',          'Administración'),
('/Usuario/Create',                 'Usuarios',          'Administración'),
('/Usuario/Edit',                   'Usuarios',          'Administración'),
('/Rol/Index',                      'Roles',             'Administración'),
('/Rol/Create',                     'Roles',             'Administración'),
('/Rol/Edit',                       'Roles',             'Administración'),
-- ============================================================
-- Módulo: Compras
-- ============================================================
('/Compra/Index',                   'Órdenes de Compra', 'Compras'),
('/Proveedor/Index',                'Proveedores',       'Compras'),
('/UnidadesMedida/Index',           'Unidades de Medida','Compras'),
('/Insumo/Index',                   'Insumos',           'Compras'),
('/InsumoCategoria/Index',          'Categorías Insumo', 'Compras'),
('/MovimientoInventario/Index',     'Movimientos',       'Compras'),
('/CierreInventario/Index',         'Cierre de Inventario', 'Compras'),
-- ============================================================
-- Módulo: Caja
-- ============================================================
('/ControlCaja/Index',              'Apertura de Caja',  'Caja'),
('/ControlCaja/Apertura',           'Apertura de Caja',  'Caja'),
('/ControlCaja/Cierre',             'Cierre de Caja',    'Caja'),
('/EgresoCaja/Index',               'Egresos',           'Caja'),
('/Gastos/Index',                   'Gastos operativos', 'Caja'),
-- ============================================================
-- Módulo: Ventas
-- ============================================================
('/Venta/Index',                    'Ventas',            'Ventas'),
('/Cliente/Index',                  'Clientes',          'Ventas'),
('/Producto/Index',                 'Productos',         'Ventas'),
('/ProductoCategoria/Index',        'Categorías',        'Ventas'),
('/MetodoPago/Index',               'Método de Pago',    'Ventas');
GO

-- ================================================================
-- ROL PERMISOS
-- ================================================================
-- ADMINISTRADOR: todos los permisos (asumiendo IDs 1-20)
INSERT INTO RolPermisos (RolId, PermisosId)
VALUES
    (1, 1),   -- Dashboard
    (1, 2),   -- Negocio
    (1, 3),   -- Usuario
    (1, 4),   -- Rol
    (1, 5),   -- Compra
    (1, 6),   -- Proveedor
    (1, 7),   -- Unidades de medida
    (1, 8),   -- Insumos
    (1, 9),   -- Categorías insumo
    (1, 10),  -- Movimientos del inventario
    (1, 11),  -- Cierre de inventario
    (1, 12),  -- Caja
    (1, 13),  -- Apertura de caja
    (1, 14),  -- Cierre de caja
    (1, 15),  -- Egresos de caja
    (1, 16),  -- Gastos operativos
    (1, 17),  -- Venta
    (1, 18),  -- Clientes
    (1, 19),  -- Productos
    (1, 20),  -- Categoría de productos
    (1, 21);  -- Metodo de pago
GO

-- CAJERO: solo Ventas, Clientes, Productos, Categorías, Método de Pago, Caja
INSERT INTO RolPermisos (RolId, PermisosId)
VALUES
    (2,  1),  -- Dashboard
    (2, 12),  -- Caja
    (2, 13),  -- Apertura de caja
    (2, 14),  -- Cierre de caja
    (2, 15),  -- Egresos de caja
    (2, 17),  -- Venta
    (2, 18),  -- Clientes
    (2, 19),  -- Productos
    (2, 20),  -- Categoría de productos
    (2, 21);  -- Metodo de pago
GO
