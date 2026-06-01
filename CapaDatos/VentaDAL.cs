using Dapper;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaDatos
{
    public class VentaDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public List<Venta> Listar()
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<Venta>(
                "sp_ListarVentas",
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public Venta ObtenerPorId(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            return Conexion.QueryFirstOrDefault<Venta>(
                "sp_ObtenerVentaPorId",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<DetalleVenta> ObtenerDetallesVenta(int ventaId)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<DetalleVenta>(
                "sp_ObtenerDetallesVenta",
                new { VentaId = ventaId },
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public List<DetallePago> ObtenerDetallesPago(int ventaId)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<DetallePago>(
                "sp_ObtenerDetallesPago",
                new { VentaId = ventaId },
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public int Crear(int? clienteId, int usuarioId, decimal total, bool enLocal, bool? platoPrestado, decimal montoRecibido, decimal cambioDevuelto, List<DetalleVenta> detallesVenta, List<DetallePago> detallesPago)
        {
            DataTable dtDetalles = new DataTable();
            dtDetalles.Columns.Add("ProductoId", typeof(int));
            dtDetalles.Columns.Add("Cantidad", typeof(int));
            dtDetalles.Columns.Add("PrecioUnitario", typeof(decimal));
            dtDetalles.Columns.Add("SubTotal", typeof(decimal));

            foreach (var det in detallesVenta)
            {
                dtDetalles.Rows.Add(det.ProductoId, det.Cantidad, det.PrecioUnitario, det.SubTotal);
            }

            DataTable dtPagos = new DataTable();
            dtPagos.Columns.Add("MetodoPagoId", typeof(int));
            dtPagos.Columns.Add("Monto", typeof(decimal));

            foreach (var pago in detallesPago)
            {
                dtPagos.Rows.Add(pago.MetodoPagoId, pago.Monto);
            }

            using var Conexion = conexion.ObtenerConexion();
            
            var parametros = new DynamicParameters();
            parametros.Add("@ClienteId", clienteId);
            parametros.Add("@UsuarioId", usuarioId);
            parametros.Add("@Total", total);
            parametros.Add("@EnLocal", enLocal);
            parametros.Add("@PlatoPrestado", platoPrestado);
            parametros.Add("@MontoRecibido", montoRecibido);
            parametros.Add("@CambioDevuelto", cambioDevuelto);
            parametros.Add("@Detalles", dtDetalles.AsTableValuedParameter("DetalleVentaTipo"));
            parametros.Add("@Pagos", dtPagos.AsTableValuedParameter("DetallePagoTipo"));

            return Conexion.QuerySingle<int>(
                "sp_CrearVenta",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public bool Anular(int id, int usuarioId)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_AnularVenta",
                new { Id = id, UsuarioId = usuarioId },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool DevolverPlatos(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_DevolverPlatos",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }
    }
}
