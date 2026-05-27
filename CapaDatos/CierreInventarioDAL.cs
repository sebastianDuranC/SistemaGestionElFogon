using Dapper;
using Entidades;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaDatos
{
    public class CierreInventarioDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public List<CierreInventario> ObtenerTodos()
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<CierreInventario>(
                "sp_ListarCierresInventario",
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public bool RegistrarCierre(List<CierreInventario> cierres, int usuarioId)
        {
            var tabla = new DataTable();
            tabla.Columns.Add("InsumoId", typeof(int));
            tabla.Columns.Add("CantidadTeorica", typeof(decimal));
            tabla.Columns.Add("CantidadReal", typeof(decimal));
            tabla.Columns.Add("Diferencia", typeof(decimal));
            tabla.Columns.Add("Observacion", typeof(string));

            foreach (var item in cierres)
            {
                tabla.Rows.Add(item.InsumoId, item.CantidadTeorica, item.CantidadReal, item.Diferencia, item.Observacion);
            }

            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_RegistrarCierreInventario",
                new
                {
                    Cierres = tabla.AsTableValuedParameter("CierreInventarioTipo"),
                    UsuarioId = usuarioId
                },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }
    }
}
