using Dapper;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaDatos
{
    public class CompraDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public List<Compra> Listar()
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<Compra>(
                "sp_ListarCompras",
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public Compra ObtenerPorId(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            return Conexion.QueryFirstOrDefault<Compra>(
                "sp_ObtenerCompraPorId",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<DetalleCompra> ObtenerDetalles(int compraId)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<DetalleCompra>(
                "sp_ObtenerDetallesCompra",
                new { CompraId = compraId },
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public int Crear(int proveedorId, int usuarioId, decimal total, List<DetalleCompra> detalles)
        {
            DataTable dtDetalles = new DataTable();
            dtDetalles.Columns.Add("InsumoId", typeof(int));
            dtDetalles.Columns.Add("Cantidad", typeof(decimal));
            dtDetalles.Columns.Add("CostoUnitario", typeof(decimal));
            dtDetalles.Columns.Add("Subtotal", typeof(decimal));

            foreach (var det in detalles)
            {
                dtDetalles.Rows.Add(det.InsumoId, det.Cantidad, det.CostoUnitario, det.Subtotal);
            }

            using var Conexion = conexion.ObtenerConexion();
            
            var parametros = new DynamicParameters();
            parametros.Add("@ProveedorId", proveedorId);
            parametros.Add("@UsuarioId", usuarioId);
            parametros.Add("@Total", total);
            parametros.Add("@Detalles", dtDetalles.AsTableValuedParameter("DetalleCompraTipo"));

            return Conexion.QuerySingle<int>(
                "sp_CrearCompra",
                parametros,
                commandType: CommandType.StoredProcedure
            );
        }

        public bool Anular(int id, int usuarioId)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultdo = Conexion.Execute(
                "sp_AnularCompra",
                new { Id = id, UsuarioId = usuarioId },
                commandType: CommandType.StoredProcedure
            );
            return resultdo > 0;
        }
    }
}
