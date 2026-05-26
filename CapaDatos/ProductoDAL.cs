using Dapper;
using Entidades;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaDatos
{
    public class ProductoDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public List<Producto> ObtenerTodos()
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<Producto>(
                "sp_ListarProductos",
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public Producto ObtenerPorId(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            return Conexion.QueryFirstOrDefault<Producto>(
                "sp_ObtenerProductoPorId",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public bool CrearProducto(Producto producto, List<ProductoInsumo> insumos)
        {
            using var Conexion = conexion.ObtenerConexion();
            var recetaTable = new DataTable();
            recetaTable.Columns.Add("InsumoId", typeof(int));
            recetaTable.Columns.Add("Cantidad", typeof(decimal));
            recetaTable.Columns.Add("Tipo", typeof(string));

            if (insumos != null)
            {
                foreach (var insumo in insumos)
                {
                    recetaTable.Rows.Add(insumo.InsumoId, insumo.Cantidad, insumo.Tipo);
                }
            }

            int productoId = Conexion.QuerySingle<int>(
                "sp_CrearProducto",
                new
                {
                    producto.Nombre,
                    producto.Precio,
                    producto.FotoUrl,
                    producto.ProductoCategoriaId,
                    Receta = recetaTable.AsTableValuedParameter("RecetaInsumoTipo")
                },
                commandType: CommandType.StoredProcedure
            );
            return productoId > 0;
        }

        public bool EditarProducto(Producto producto, List<ProductoInsumo> insumos)
        {
            using var Conexion = conexion.ObtenerConexion();
            var recetaTable = new DataTable();
            recetaTable.Columns.Add("InsumoId", typeof(int));
            recetaTable.Columns.Add("Cantidad", typeof(decimal));
            recetaTable.Columns.Add("Tipo", typeof(string));

            if (insumos != null)
            {
                foreach (var insumo in insumos)
                {
                    recetaTable.Rows.Add(insumo.InsumoId, insumo.Cantidad, insumo.Tipo);
                }
            }

            var resultado = Conexion.Execute(
                "sp_EditarProducto",
                new
                {
                    producto.Id,
                    producto.Nombre,
                    producto.Precio,
                    producto.FotoUrl,
                    producto.ProductoCategoriaId,
                    Receta = recetaTable.AsTableValuedParameter("RecetaInsumoTipo")
                },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool EliminarProducto(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EliminarProducto",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }
    }
}
