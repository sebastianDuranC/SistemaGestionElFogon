using CapaDatos;
using System;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class RolPermisoBLL
    {
        private readonly RolPermisoDAL rolPermisoDAL = new RolPermisoDAL();

        public List<int> ObtenerPermisosPorRol(int rolId)
        {
            if (rolId <= 0) return new List<int>();
            return rolPermisoDAL.ObtenerPermisosPorRol(rolId);
        }

        public void ActualizarRolPermisos(int rolId, List<int> permisosIds)
        {
            if (rolId <= 0)
            {
                throw new ArgumentException("El ID del rol es inválido");
            }
            rolPermisoDAL.ActualizarRolPermisos(rolId, permisosIds);
        }
    }
}
