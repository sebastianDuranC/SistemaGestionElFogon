using Dapper;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaDatos
{
    public class RolPermisoDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public List<int> ObtenerPermisosPorRol(int rolId)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<int>(
                "sp_ObtenerPermisosPorRol",
                new { RolId = rolId },
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public void ActualizarRolPermisos(int rolId, List<int> permisosIds)
        {
            using var Conexion = conexion.ObtenerConexion();
            string permisosStr = permisosIds != null ? string.Join(",", permisosIds) : "";
            Conexion.Execute(
                "sp_ActualizarRolPermisos",
                new { RolId = rolId, PermisosIds = permisosStr },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
