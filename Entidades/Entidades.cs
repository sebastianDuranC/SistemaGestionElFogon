using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades
{
    public class Negocio
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
    public class Permiso
    {
        public int Id { get; set; }
        public string FormNombre { get; set; }
        public string FormRuta { get; set; }
        public string Modulo { get; set; }
        public bool Estado { get; set; }
    }
    public class RolPermiso
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public int PermisosId { get; set; }
        public bool Estado { get; set; }
    }
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Contra { get; set; }   // hash bcrypt
        public int RolId { get; set; }
        public string NombreRol { get; set; }
        public int NegocioId { get; set; }
        public bool Estado { get; set; }
    }
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public bool EsComerciante { get; set; }
        public string NumeroLocal { get; set; }
        public string Pasillo { get; set; }
        public bool Estado { get; set; }
    }
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int ProductoCategoriaId { get; set; }
        public string NombreCategoria { get; set; }
        public string FotoUrl { get; set; }
        public int Stock { get; set; }             // calculado en el SP
        public bool Estado { get; set; }
    }
    public class ProductoCategoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
    public class Proveedor
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Contacto { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
    public class Insumo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Costo { get; set; }
        public decimal Stock { get; set; }
        public decimal StockMinimo { get; set; }
        public int InsumoCategoriaId { get; set; }
        public string NombreCategoria { get; set; }
        public int ProveedorId { get; set; }
        public string NombreProveedor { get; set; }
        public string FotoUrl { get; set; }
        public int UnidadesMedidaId { get; set; }
        public string NombreMedidas { get; set; }
        public bool Estado { get; set; }
    }
    public class InsumoCategoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
    public class ProductoInsumo
    {
        public int InsumoId { get; set; }
        public string InsumoNombre { get; set; }
        public decimal Cantidad { get; set; }
        public string Tipo { get; set; }        // "Comestible" o "Descartable"
    }
    public class UnidadMedida
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Abreviatura { get; set; }
        public bool Estado { get; set; }
    }
    public class MetodoPago
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
    public class Venta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public bool EnLocal { get; set; }
        public string TipoVenta { get; set; }    // "En Local" / "Para Llevar" — del SP
        public int? ClienteId { get; set; }
        public string Cliente { get; set; }
        public int UsuarioId { get; set; }
        public string Vendedor { get; set; }
        public int MetodoPagoId { get; set; }
        public string MetodoPago { get; set; }
        public decimal MontoRecibido { get; set; }
        public decimal CambioDevuelto { get; set; }
        public bool Estado { get; set; }
    }
    public class DetalleVenta
    {
        public int Id { get; set; }
        public int VentaId { get; set; }
        public int ProductoId { get; set; }
        public string ProductoNombre { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal SubTotal { get; set; }
        public bool Estado { get; set; }
    }
    public class Compra
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public int ProveedorId { get; set; }
        public string NombreProveedor { get; set; }
        public bool Estado { get; set; }
    }
    public class DetalleCompra
    {
        public int Id { get; set; }
        public int CompraId { get; set; }
        public int InsumoId { get; set; }
        public string NombreInsumo { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Costo { get; set; }
        public bool Estado { get; set; }
    }
    public class MovimientoInventario
    {
        public int Id { get; set; }
        public int InsumoId { get; set; }
        public string NombreInsumo { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoMovimiento { get; set; } // "Entrada","Salida","Anulación"
        public decimal Cantidad { get; set; }
        public string Observacion { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public bool Estado { get; set; }
    }

    public class ControlCaja
    {
        public int Id { get; set; }
        public DateTime FechaHoraApertura { get; set; }
        public decimal MontoApertura { get; set; }
        public DateTime? FechaHoraCierre { get; set; }
        public decimal? MontoCierreEsperado { get; set; }
        public decimal? MontoCierreReal { get; set; }
        public decimal? Diferencial { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public int NegocioId { get; set; }
        public bool Estado { get; set; }
    }

    public class GastoOperativo
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Concepto { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public bool Estado { get; set; }
    }

    public class EgresosCaja
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public int ControlCajaId { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }

    public class CierreInventario
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public decimal CantidadTeorica { get; set; }
        public decimal CantidadReal { get; set; }
        public decimal Diferencia { get; set; }
        public string Observacion { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public int InsumoId { get; set; }
        public string NombreInsumo { get; set; } = string.Empty;
        public int ControlCajaId { get; set; }
        public bool Estado { get; set; }
    }
}
