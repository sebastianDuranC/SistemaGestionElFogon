using Dapper;
using Entidades;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CapaDatos
{
    public class RolDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public List<Rol> ObtenerTodos()
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<Rol>(
                "sp_ListarRoles",
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public Rol ObtenerPorId(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            return Conexion.QueryFirstOrDefault<Rol>(
                "sp_ObtenerRolPorId",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public int CrearRol(Rol nuevoRol)
        {
            using var Conexion = conexion.ObtenerConexion();
            return Conexion.QuerySingle<int>(
                "sp_CrearRol",
                new { nuevoRol.Nombre, nuevoRol.Estado },
                commandType: CommandType.StoredProcedure
            );
        }

        public bool EditarRol(Rol rolEditado)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EditarRol",
                new { rolEditado.Id, rolEditado.Nombre, rolEditado.Estado },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool EliminarRol(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EliminarRol",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }
    }
}
