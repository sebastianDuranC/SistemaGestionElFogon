-- BD PARA SISTEMA DE VENTA EL FOGON CON ASP.NET WEB FORM
CREATE DATABASE Prueba;
GO
USE Prueba;
GO
CREATE TABLE Negocio (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(150) NOT NULL,
    Direccion NVARCHAR(250) NULL,
    LogoUrl NVARCHAR(MAX) NULL,
    Estado BIT NOT NULL DEFAULT 1
);

CREATE TABLE Rol (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1
);

CREATE TABLE Usuario (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Contra NVARCHAR(300) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1,
    NegocioId INT NOT NULL,
    RolId INT NOT NULL,
    FOREIGN KEY (NegocioId) REFERENCES Negocio(Id),
    FOREIGN KEY (RolId) REFERENCES Rol(Id)
);

CREATE TABLE Permisos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Modulo NVARCHAR(100) NOT NULL,
    FormNombre NVARCHAR(100) NOT NULL,
    FormRuta NVARCHAR(100) NOT NULL, -- Page/FormsAccess.aspx, Page/Login.aspx
    Estado BIT NOT NULL DEFAULT 1
);

CREATE TABLE RolPermisos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Estado BIT NOT NULL DEFAULT 1,
    RolId INT NOT NULL,
    PermisosId INT NOT NULL,
    FOREIGN KEY (RolId) REFERENCES Rol(Id),
    FOREIGN KEY (PermisosId) REFERENCES Permisos(Id)
);

CREATE TABLE ProductoCategoria (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1
);

CREATE TABLE Proveedor (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(150) NOT NULL,
    Apellido NVARCHAR(150) NOT NULL,
    Contacto NVARCHAR(100) NULL,
    Estado BIT NOT NULL DEFAULT 1
);

CREATE TABLE Producto (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(150) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL,
    FotoUrl NVARCHAR(300) NULL,
    Estado BIT NOT NULL DEFAULT 1,
    ProductoCategoriaId INT NOT NULL,
    FOREIGN KEY (ProductoCategoriaId) REFERENCES ProductoCategoria(Id)
);

CREATE TABLE Cliente (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(150) NOT NULL,
    Apellido NVARCHAR(150) NOT NULL,
    EsComerciante BIT NOT NULL DEFAULT 0, --1= Cliente de tipo Comerciante, 0= Cliente normal
    NumeroLocal NVARCHAR(20) NULL,
    Pasillo NVARCHAR(50) NULL,
    Estado BIT NOT NULL DEFAULT 1
);

CREATE TABLE MetodoPago (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1
);

CREATE TABLE Venta (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Fecha DATETIME NOT NULL DEFAULT GETDATE(), --año, mes, día, hora, minutos
    Total DECIMAL(10,2) NOT NULL,
    EnLocal BIT NOT NULL, -- 1 = En local, 0 = Para llevar
    PlatoPrestado BIT NULL, --Si el cliente es de tipo comercial y se lleva el producto hay opcion de que se lleve en plato ceramica
    MontoRecibido DECIMAL(10,2) NOT NULL,
    CambioDevuelto DECIMAL(10,2) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1,
    ClienteId INT NULL,
    UsuarioId INT NOT NULL,
    FOREIGN KEY (ClienteId) REFERENCES Cliente(Id),
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id)
);

CREATE TABLE DetallePago (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Monto DECIMAL(10,2) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1,
    VentaId INT NOT NULL,
    MetodoPagoId INT NOT NULL,
    FOREIGN KEY (VentaId) REFERENCES Venta(Id),
    FOREIGN KEY (MetodoPagoId) REFERENCES MetodoPago(Id)
);

CREATE TABLE DetalleVenta (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL NOT NULL,
    SubTotal DECIMAL(10,2) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1,
    VentaId INT NOT NULL,
    ProductoId INT NOT NULL,
    FOREIGN KEY (VentaId) REFERENCES Venta(Id),
    FOREIGN KEY (ProductoId) REFERENCES Producto(Id)
);

CREATE TABLE Compra (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Total DECIMAL(10,2) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1,
    UsuarioId INT NOT NULL,
    ProveedorId INT NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id),
    FOREIGN KEY (ProveedorId) REFERENCES Proveedor(Id)
);

CREATE TABLE UnidadesMedida (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(50) NOT NULL,
    Abreviatura NVARCHAR(50) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1		
);

CREATE TABLE InsumoCategoria(
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1
);

CREATE TABLE Insumo (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(150) NOT NULL,
    Costo DECIMAL(10,2) NULL DEFAULT 0.00,
    Stock DECIMAL(10,2) NULL DEFAULT 0.00, --stock actual
    StockMinimo DECIMAL(10,2) NULL DEFAULT 0.00,
    FotoUrl NVARCHAR(MAX) NULL,
    Estado BIT NOT NULL DEFAULT 1,
    InsumoCategoriaId INT NOT NULL,
    ProveedorId INT NOT NULL,
    UnidadesMedidaId INT NOT NULL DEFAULT 1,
    FOREIGN KEY (InsumoCategoriaId) REFERENCES InsumoCategoria(Id),
    FOREIGN KEY (ProveedorId) REFERENCES Proveedor(Id),
    FOREIGN KEY (UnidadesMedidaId) REFERENCES UnidadesMedida(Id)
);

CREATE TABLE DetalleCompra (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Cantidad DECIMAL(10,2) NOT NULL,
    CostoUnitario DECIMAL(10,2) NOT NULL,
    Subtotal DECIMAL(10,2) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1,
    CompraId INT NOT NULL,
    InsumoId INT NOT NULL,
    FOREIGN KEY (CompraId) REFERENCES Compra(Id),
    FOREIGN KEY (InsumoId) REFERENCES Insumo(Id)
);

CREATE TABLE MovimientoInventario (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    TipoMovimiento NVARCHAR(50) NOT NULL, -- Entrada, Salida, Daño, UsoInterno
    Cantidad DECIMAL(10,2) NOT NULL,
    Observacion NVARCHAR(300) NULL,
    Estado BIT NOT NULL DEFAULT 1,
    InsumoId INT NOT NULL,
    UsuarioId INT NOT NULL,
    FOREIGN KEY (InsumoId) REFERENCES Insumo(Id),
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id)
);

--control de insumo en un plato(carnes) y que plato y vaso va usar(descartables)
CREATE TABLE ProductoInsumo (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Cantidad DECIMAL(10,2) NOT NULL,
    Tipo NVARCHAR(20) NOT NULL CHECK (Tipo IN ('Comestible', 'Descartable')),
    Estado BIT NOT NULL DEFAULT 1,
    ProductoId INT NOT NULL,
    InsumoId INT NOT NULL,
    FOREIGN KEY (ProductoId) REFERENCES Producto(Id),
    FOREIGN KEY (InsumoId) REFERENCES Insumo(Id)
);

-- =======================================
-- Modulo de Gastos y caja
-- =======================================
-- Bitacora de apertura y cierre de turno
-- Turno del cajero
CREATE TABLE ControlCaja (
    Id                  INT IDENTITY(1,1) PRIMARY KEY,
    FechaHoraApertura   DATETIME NOT NULL DEFAULT GETDATE(),
    MontoApertura       DECIMAL(10,2) NOT NULL,
    FechaHoraCierre     DATETIME NULL,
    MontoCierreEsperado DECIMAL(10,2) NULL,
    MontoCierreReal     DECIMAL(10,2) NULL,
    Diferencial         DECIMAL(10,2) NULL,
    Estado              BIT NOT NULL DEFAULT 1,
    UsuarioId           INT NOT NULL,
    NegocioId           INT NOT NULL,
    FOREIGN KEY (UsuarioId)  REFERENCES Usuario(Id),
    FOREIGN KEY (NegocioId)  REFERENCES Negocio(Id)
);

-- Egresos fsicos del cajn durante el turno (cajero)
-- Ejemplo: llega proveedor y le pagan del cajn
CREATE TABLE EgresosCaja (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    Fecha           DATETIME NOT NULL DEFAULT GETDATE(),
    Motivo          NVARCHAR(150) NOT NULL,
    Monto           DECIMAL(10,2) NOT NULL,
    Estado          BIT NOT NULL DEFAULT 1,
    ControlCajaId   INT NOT NULL,
    UsuarioId       INT NOT NULL,
    FOREIGN KEY (ControlCajaId) REFERENCES ControlCaja(Id),
    FOREIGN KEY (UsuarioId)     REFERENCES Usuario(Id)
);

-- Gastos del negocio que el admin paga antes del turno (no tocan el cajn)
-- Ejemplo: aceite, gas, pasaje al mercado
CREATE TABLE GastosOperativos (
    Id          INT IDENTITY(1,1) PRIMARY KEY,
    Fecha       DATETIME NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    Concepto    NVARCHAR(100) NOT NULL,
    Monto       DECIMAL(10,2) NOT NULL,
    Estado      BIT NOT NULL DEFAULT 1,
    UsuarioId   INT NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id)
);

CREATE TABLE CierreInventario (
    Id int NOT NULL IDENTITY(1,1),
    CantidadTeorica decimal(10, 2) NOT NULL,
    CantidadReal decimal(10, 2) NOT NULL,
    Diferencia decimal(10, 2) NOT NULL,
    Observacion nvarchar(300),
    FechaHora datetime NOT NULL DEFAULT GETDATE(),
    Estado BIT NOT NULL DEFAULT 1,
    UsuarioId int NOT NULL,
    InsumoId int NOT NULL,
    FOREIGN KEY (UsuarioId) REFERENCES Usuario(Id),
    FOREIGN KEY (InsumoId) REFERENCES Insumo(Id)
);