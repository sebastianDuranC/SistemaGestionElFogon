USE Prueba;
GO

INSERT INTO Negocio (Nombre, Direccion, LogoUrl)
VALUES ('El Fogón', 'Ramada', '');
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
       ('Maria', '$2a$11$Zhs4wRW6CUXxOUOvIJczNuWcKIMPPZNR.XudrE5VfWKJ1FD.Z0ECy', 1, 2);
GO

-- ================================================================
-- PERMISOS (rutas de Razor Pages)
-- ================================================================
INSERT INTO Permisos (FormRuta, FormNombre, Modulo)
VALUES
-- ============================================================
-- Dashboard
-- ============================================================
('/Index',                          'Dashboard',         'Vista general del negocio'),
-- ============================================================
-- Módulo: Acceso y Administración
-- ============================================================
('/Negocio/Edit',                   'Edición de Negocio',           'Administración'),

('/Usuario/Index',                  'Gestión de Usuarios',          'Administración'),
('/Usuario/Create',                 'Registro de Usuarios',          'Administración'),
('/Usuario/Edit',                   'Edición de Usuarios',          'Administración'),

('/Rol/Index',                      'Gestión de Roles',             'Administración'),
('/Rol/Create',                     'Registro de Roles',             'Administración'),
('/Rol/Edit',                       'Edición de Roles',             'Administración'),
-- ============================================================
-- Módulo: Compras
-- ============================================================
('/Compra/Index',                   'Gestión de Compra', 'Compras'),
('/Compra/Create',                  'Registro de Órdenes de Compra', 'Compras'),
('/Compra/Detalle',                 'Detalle de Órdenes de Compra', 'Compras'),

('/Proveedor/Index',                'Gestión de Proveedores',       'Compras'),
('/Proveedor/Create',               'Registro de Proveedores',       'Compras'),
('/Proveedor/Edit',                 'Edición de Proveedores',       'Compras'),

('/UnidadesMedida/Index',           'Gestión de Unidades de Medida','Compras'),
('/UnidadesMedida/Create',          'Registro de Unidades de Medida','Compras'),
('/UnidadesMedida/Edit',            'Edición de Unidades de Medida','Compras'),

('/Insumo/Index',                   'Gestión de Insumos',           'Compras'),
('/Insumo/Create',                  'Registro de Insumos',           'Compras'),
('/Insumo/Edit',                    'Edición de Insumos',           'Compras'),

('/InsumoCategoria/Index',          'Gestión de Categorías de Insumo', 'Compras'),
('/InsumoCategoria/Create',         'Registro de Categorías de Insumo', 'Compras'),
('/InsumoCategoria/Edit',           'Edición de Categorías de Insumo', 'Compras'),

('/MovimientoInventario/Index',     'Movimientos de compra y venta',       'Compras'),
('/MovimientoInventario/Create',    'Registro de Movimientos de compra y venta',       'Compras'),

('/CierreInventario/Index',         'Cierre de Inventario', 'Compras'),
('/CierreInventario/Create',        'Registro de Cierre de Inventario', 'Compras'),
-- ============================================================
-- Módulo: Caja
-- ============================================================
('/ControlCaja/Index',              'Gestión de Caja',   'Caja'),
('/ControlCaja/Apertura',           'Apertura de Caja',   'Caja'),
('/ControlCaja/Cierre',             'Cierre de Caja',     'Caja'),

('/EgresoCaja/Index',               'Gestión de Egresos',            'Caja'),
('/EgresoCaja/Create',              'Registro de Egresos',            'Caja'),

('/Gastos/Index',                   'Gestión de Gastos operativos',  'Caja'),
('/Gastos/Create',                  'Registro de Gastos operativos',  'Caja'),
('/Gastos/Edit',                    'Edición de Gastos operativos',  'Caja'),
-- ============================================================
-- Módulo: Ventas
-- ============================================================
('/Venta/Index',                    'Gestión de Ventas',            'Ventas'),
('/Venta/Create',                   'Registro de Ventas',            'Ventas'),

('/Cliente/Index',                  'Gestión de Clientes',          'Ventas'),
('/Cliente/Create',                 'Registro de Clientes',          'Ventas'),
('/Cliente/Edit',                   'Edición de Clientes',          'Ventas'),

('/Producto/Index',                 'Gestión de Productos',         'Ventas'),
('/Producto/Create',                'Registro de Productos',         'Ventas'),
('/Producto/Edit',                  'Edición de Productos',         'Ventas'),

('/ProductoCategoria/Index',        'Gestión de Categorías de Productos',        'Ventas'),
('/ProductoCategoria/Create',       'Registro de Categorías de Productos',        'Ventas'),
('/ProductoCategoria/Edit',         'Edición de Categorías de Productos',        'Ventas'),

('/MetodoPago/Index',               'Gestión de Método de Pago',    'Ventas'),
('/MetodoPago/Create',              'Registro de Método de Pago',    'Ventas'),
('/MetodoPago/Edit',                'Edición de Método de Pago',    'Ventas');
GO

-- ================================================================
-- ROL PERMISOS
-- ================================================================
-- ADMINISTRADOR: todos los permisos
INSERT INTO RolPermisos (RolId, PermisosId)
VALUES
    -- Dashboard
    (1, 1),   -- Dashboard

    -- Módulo: Acceso y Administración
    (1, 2),   -- Edición de Negocio
    (1, 3),   -- Gestión de Usuarios
    (1, 4),   -- Registro de Usuarios
    (1, 5),   -- Edición de Usuarios
    (1, 6),   -- Gestión de Roles
    (1, 7),   -- Registro de Roles
    (1, 8),   -- Edición de Roles

    -- Módulo: Compras
    (1, 9),   -- Gestión de Compra
    (1, 10),  -- Registro de Órdenes de Compra
    (1, 11),  -- Detalle de Órdenes de Compra
    (1, 12),  -- Gestión de Proveedores
    (1, 13),  -- Registro de Proveedores
    (1, 14),  -- Edición de Proveedores
    (1, 15),  -- Gestión de Unidades de Medida
    (1, 16),  -- Registro de Unidades de Medida
    (1, 17),  -- Edición de Unidades de Medida
    (1, 18),  -- Gestión de Insumos
    (1, 19),  -- Registro de Insumos
    (1, 20),  -- Edición de Insumos
    (1, 21),  -- Gestión de Categorías de Insumo
    (1, 22),  -- Registro de Categorías de Insumo
    (1, 23),  -- Edición de Categorías de Insumo
    (1, 24),  -- Movimientos de compra y venta
    (1, 25),  -- Registro de Movimientos
    (1, 26),  -- Cierre de Inventario
    (1, 27),  -- Registro de Cierre de Inventario

    -- Módulo: Caja
    (1, 28),  -- Gestión de Caja
    (1, 29),  -- Apertura de Caja
    (1, 30),  -- Cierre de Caja
    (1, 31),  -- Gestión de Egresos
    (1, 32),  -- Registro de Egresos
    (1, 33),  -- Gestión de Gastos operativos
    (1, 34),  -- Registro de Gastos operativos
    (1, 35),  -- Edición de Gastos operativos

    -- Módulo: Ventas
    (1, 36),  -- Gestión de Ventas
    (1, 37),  -- Registro de Ventas
    (1, 38),  -- Gestión de Clientes
    (1, 39),  -- Registro de Clientes
    (1, 40),  -- Edición de Clientes
    (1, 41),  -- Gestión de Productos
    (1, 42),  -- Registro de Productos
    (1, 43),  -- Edición de Productos
    (1, 44),  -- Gestión de Categorías de Productos
    (1, 45),  -- Registro de Categorías de Productos
    (1, 46),  -- Edición de Categorías de Productos
    (1, 47),  -- Gestión de Método de Pago
    (1, 48),  -- Registro de Método de Pago
    (1, 49);  -- Edición de Método de Pago
GO

-- CAJERO: solo Ventas, Clientes, Productos, Categorías, Método de Pago, Caja
INSERT INTO RolPermisos (RolId, PermisosId)
VALUES
    (2,  1),  -- Dashboard
    (1, 28),  -- Gestión de Caja
    (1, 29),  -- Apertura de Caja
    (1, 30),  -- Cierre de Caja
    (1, 31),  -- Gestión de Egresos
    (1, 32),  -- Registro de Egresos
    (1, 36),  -- Gestión de Ventas
    (1, 37),  -- Registro de Ventas
    (1, 38),  -- Gestión de Clientes
    (1, 39),  -- Registro de Clientes
    (1, 40);  -- Edición de Clientes
GO

INSERT INTO ProductoCategoria (Nombre, Estado) 
VALUES 
    ('Platos', 1),
    ('Gaseosas', 1),
    ('Refrescos', 1);
GO

INSERT INTO Proveedor (Nombre, Apellido, Contacto, Estado) 
VALUES 
    ('Jose', 'Covarrubias', '61316555', 1),
    ('Daniel', 'Perez', '76504224', 1),
    ('Maria', 'Gonzalez', '4234223', 1),
    ('Ruben', 'Martinez', '61316566', 1);
GO

INSERT INTO Producto (Nombre, Precio, ProductoCategoriaId, Estado) VALUES 
    ('Pollo plancha 1/4', 19.00, 1, 1);
GO

INSERT INTO Cliente (Nombre, Apellido, EsComerciante, NumeroLocal, Pasillo, Estado) 
VALUES 
    ('Cliente normal', '', 0, '', '', 1),
    ('Pedro', 'Gonzalez', 1, '15', 'D', 1),
    ('Jose', 'Perez', 1, '13', 'C', 1);
GO

INSERT INTO MetodoPago (Nombre, Estado) VALUES 
    ('Efectivo', 1),
    ('Qr', 1);
GO

INSERT INTO UnidadesMedida (Nombre, Abreviatura, Estado) VALUES 
    ('Kilogramos', 'Kg', 1),
    ('Unidad', 'Un', 1);
GO

INSERT INTO InsumoCategoria (Nombre, Estado) 
VALUES 
    ('Carnes', 1),
    ('Vegetales', 1),
    ('Descartables', 1);
GO  

INSERT INTO Insumo (Nombre, InsumoCategoriaId, ProveedorId, UnidadesMedidaId, Costo, Stock, StockMinimo, Estado) 
VALUES 
    ('Pollo', 1, 1, 2, 0.00, 0.00, 0.00, 1),
    ('Botella Coca Cola 2l', 3, 3, 2, 0.00, 0.00, 0.00, 1),
    ('Vaso plastico', 3, 2, 2, 0.00, 0.00, 0.00, 1);
GO

INSERT INTO ProductoInsumo (ProductoId, InsumoId, Cantidad, Tipo, Estado) 
VALUES 
    (1, 1, 0.25, 'Comestible', 1);
GO