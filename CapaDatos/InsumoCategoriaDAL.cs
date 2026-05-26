using Dapper;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaDatos
{
    public class InsumoCategoriaDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public List<InsumoCategoria> ObtenerTodos()
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<InsumoCategoria>(
                "sp_ListarInsumoCategorias",
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public InsumoCategoria ObtenerPorId(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            return Conexion.QueryFirstOrDefault<InsumoCategoria>(
                "sp_ObtenerInsumoCategoriaPorId",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public bool Crear(InsumoCategoria categoria)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_CrearInsumoCategoria",
                new { categoria.Nombre },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool Editar(InsumoCategoria categoria)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EditarInsumoCategoria",
                new { categoria.Id, categoria.Nombre },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool Eliminar(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EliminarInsumoCategoria",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }
    }
}
