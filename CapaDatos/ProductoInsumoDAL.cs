using Dapper;
using Entidades;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaDatos
{
    public class ProductoInsumoDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public List<ProductoInsumo> ObtenerInsumosPorProducto(int productoId)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<ProductoInsumo>(
                "sp_ObtenerInsumosPorProducto",
                new { ProductoId = productoId },
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }
    }
}
