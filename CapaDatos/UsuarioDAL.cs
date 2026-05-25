using Dapper;
using Entidades;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CapaDatos
{
    public class UsuarioDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public List<Usuario> ObtenerTodos()
        {
            List<Usuario> Lista = new List<Usuario>();

            using var Conexion = conexion.ObtenerConexion();
            var resultado =Conexion.Query<Usuario>(
                "sp_ListarUsuarios",
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public Usuario ObtenerPorNombre(string Nombre)
        {
            using var Conexion = conexion.ObtenerConexion();
            return Conexion.QueryFirstOrDefault<Usuario>(
                "sp_ObtenerUsuarioPorNombre",
                new { Nombre },
                commandType: CommandType.StoredProcedure
            );
        }

        public bool CrearUsuario(Usuario usuario)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_CrearUsuario",
                new
                {
                    usuario.Nombre,
                    usuario.Contra,
                    usuario.RolId,
                    usuario.NegocioId,
                    usuario.Estado
                },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool EditarUsuario(Usuario usuario)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EditarUsuario",
                new
                {
                    usuario.Id,
                    usuario.Nombre,
                    usuario.Contra,
                    usuario.RolId,
                    usuario.NegocioId,
                    usuario.Estado
                },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public Usuario ObtenerPorId(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            return Conexion.QueryFirstOrDefault<Usuario>(
                "sp_ObtenerUsuarioPorId",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public bool EliminarUsuario(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EliminarUsuario",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }
    }
}
