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
('/Index',                          'Dashboard',         'Vista general del negocio'),
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
('/Compra/Create',                  'Órdenes de Compra', 'Compras'),
('/Compra/Edit',                    'Órdenes de Compra', 'Compras'),

('/Proveedor/Index',                'Proveedores',       'Compras'),
('/Proveedor/Create',               'Proveedores',       'Compras'),
('/Proveedor/Edit',                 'Proveedores',       'Compras'),

('/UnidadesMedida/Index',           'Unidades de Medida','Compras'),
('/UnidadesMedida/Create',          'Unidades de Medida','Compras'),
('/UnidadesMedida/Edit',            'Unidades de Medida','Compras'),

('/Insumo/Index',                   'Insumos',           'Compras'),
('/Insumo/Create',                  'Insumos',           'Compras'),
('/Insumo/Edit',                    'Insumos',           'Compras'),

('/InsumoCategoria/Index',          'Categorías de Insumo', 'Compras'),
('/InsumoCategoria/Create',         'Categorías de Insumo', 'Compras'),
('/InsumoCategoria/Edit',           'Categorías de Insumo', 'Compras'),

('/MovimientoInventario/Index',     'Movimientos de compra y venta',       'Compras'),
('/MovimientoInventario/Create',    'Movimientos de compra y venta',       'Compras'),

('/CierreInventario/Index',         'Cierre de Inventario', 'Compras'),
('/CierreInventario/Create',        'Cierre de Inventario', 'Compras'),
-- ============================================================
-- Módulo: Caja
-- ============================================================
('/ControlCaja/Index',              'Apertura de Caja',   'Caja'),
('/ControlCaja/Apertura',           'Apertura de Caja',   'Caja'),
('/ControlCaja/Cierre',             'Cierre de Caja',     'Caja'),
('/EgresoCaja/Create',              'Egresos',            'Caja'),
('/Gastos/Index',                   'Gastos operativos',  'Caja'),
('/Gastos/Create',                  'Gastos operativos',  'Caja'),
-- ============================================================
-- Módulo: Ventas
-- ============================================================
('/Venta/Index',                    'Ventas',            'Ventas'),
('/Venta/Create',                   'Ventas',            'Ventas'),

('/Cliente/Index',                  'Clientes',          'Ventas'),
('/Cliente/Create',                 'Clientes',          'Ventas'),
('/Cliente/Edit',                   'Clientes',          'Ventas'),

('/Producto/Index',                 'Productos',         'Ventas'),
('/Producto/Create',                'Productos',         'Ventas'),
('/Producto/Edit',                  'Productos',         'Ventas'),

('/ProductoCategoria/Index',        'Categorías de Productos',        'Ventas'),
('/ProductoCategoria/Create',       'Categorías de Productos',        'Ventas'),
('/ProductoCategoria/Edit',         'Categorías de Productos',        'Ventas'),

('/MetodoPago/Index',               'Método de Pago',    'Ventas'),
('/MetodoPago/Create',              'Método de Pago',    'Ventas'),
('/MetodoPago/Edit',                'Método de Pago',    'Ventas');
GO

-- ================================================================
-- ROL PERMISOS
-- ================================================================
-- ADMINISTRADOR: todos los permisos (asumiendo IDs 1-20)
INSERT INTO RolPermisos (RolId, PermisosId)
VALUES
    -- Dashboard
    (1, 1),  -- Dashboard
    
    -- Módulo: Acceso y Administración
    (1, 2),  -- Negocio
    (1, 3),  -- Usuarios
    (1, 4),  -- Usuarios (Create)
    (1, 5),  -- Usuarios (Edit)
    (1, 6),  -- Roles
    (1, 7),  -- Roles (Create)
    (1, 8),  -- Roles (Edit)
    
    -- Módulo: Compras
    (1, 9),  -- Órdenes de Compra
    (1, 10), -- Órdenes de Compra (Create)
    (1, 11), -- Órdenes de Compra (Edit)
    (1, 12), -- Proveedores
    (1, 13), -- Proveedores (Create)
    (1, 14), -- Proveedores (Edit)
    (1, 15), -- Unidades de Medida
    (1, 16), -- Unidades de Medida (Create)
    (1, 17), -- Unidades de Medida (Edit)
    (1, 18), -- Insumos
    (1, 19), -- Insumos (Create)
    (1, 20), -- Insumos (Edit)
    (1, 21), -- Categorías de Insumo
    (1, 22), -- Categorías de Insumo (Create)
    (1, 23), -- Categorías de Insumo (Edit)
    (1, 24), -- Movimientos de compra y venta
    (1, 25), -- Movimientos de compra y venta (Create)
    (1, 26), -- Cierre de Inventario
    (1, 27), -- Cierre de Inventario (Create)
    
    -- Módulo: Caja
    (1, 28), -- Control de Caja
    (1, 29), -- Apertura de Caja
    (1, 30), -- Cierre de Caja
    (1, 31), -- Egresos
    (1, 32), -- Gastos operativos
    (1, 33), -- Gastos operativos (Create)
    
    -- Módulo: Ventas
    (1, 34), -- Ventas
    (1, 35), -- Ventas (Create)
    (1, 36), -- Clientes
    (1, 37), -- Clientes (Create)
    (1, 38), -- Clientes (Edit)
    (1, 39), -- Productos
    (1, 40), -- Productos (Create)
    (1, 41), -- Productos (Edit)
    (1, 42), -- Categorías de Productos
    (1, 43), -- Categorías de Productos (Create)
    (1, 44), -- Categorías de Productos (Edit)
    (1, 45), -- Método de Pago
    (1, 46), -- Método de Pago (Create)
    (1, 47); -- Método de Pago (Edit)
GO

-- CAJERO: solo Ventas, Clientes, Productos, Categorías, Método de Pago, Caja
INSERT INTO RolPermisos (RolId, PermisosId)
VALUES
    (2,  1),  -- Dashboard
    (1, 28),  -- Control de Caja
    (1, 29),  -- Apertura de Caja
    (1, 30),  -- Cierre de Caja
    (1, 31),  -- Egresos
    (1, 34),  -- Ventas
    (1, 35),  -- Ventas (Create)
    (1, 36),  -- Clientes
    (1, 37),  -- Clientes (Create)
    (1, 38);  -- Clientes (Edit)
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
    ('Ala de pollo (1/4)', 19.00, 1, 1);
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