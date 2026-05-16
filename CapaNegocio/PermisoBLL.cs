using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio
{
    public class PermisoBLL
    {
        private readonly PermisoDAL permiso = new PermisoDAL();

        // Retorna lista de rutas que el rol puede acceder
        public List<string> ObtenerRutasPermitidas(int RolId)
            => permiso.ObtenerRutasPermitidas(RolId);

        // Retorna todos los permisos (para admin al asignar permisos a roles)
        public List<Permiso> ObtenerTodos()
            => permiso.ObtenerTodos();
    }
}
