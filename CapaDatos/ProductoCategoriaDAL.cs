using Dapper;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaDatos
{
    public class ProductoCategoriaDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public List<ProductoCategoria> ObtenerTodos()
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<ProductoCategoria>(
                "sp_ListarProductoCategorias",
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public ProductoCategoria ObtenerPorId(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            return Conexion.QueryFirstOrDefault<ProductoCategoria>(
                "sp_ObtenerProductoCategoriaPorId",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public bool Crear(ProductoCategoria categoria)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_CrearProductoCategoria",
                new { categoria.Nombre },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool Editar(ProductoCategoria categoria)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EditarProductoCategoria",
                new { categoria.Id, categoria.Nombre },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool Eliminar(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EliminarProductoCategoria",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }
    }
}
