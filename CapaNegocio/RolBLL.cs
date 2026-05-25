using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio
{
    public class RolBLL
    {
        private readonly RolDAL rol = new RolDAL();
        private readonly RolPermisoBLL rolPermiso = new RolPermisoBLL();

        public List<Rol> ObtenerTodos() => rol.ObtenerTodos();

        public Rol ObtenerPorId(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            return rol.ObtenerPorId(id);
        }

        public int CrearRol(Rol nuevoRol, List<int> permisoIds)
        {
            if (nuevoRol == null || string.IsNullOrWhiteSpace(nuevoRol.Nombre))
            {
                throw new ArgumentException("El nombre del rol es obligatorio");
            }

            nuevoRol.Nombre = nuevoRol.Nombre.Trim();
            nuevoRol.Estado = true;

            int rolId = rol.CrearRol(nuevoRol);

            if (permisoIds != null && permisoIds.Count > 0)
            {
                rolPermiso.ActualizarRolPermisos(rolId, permisoIds);
            }

            return rolId;
        }

        public bool EditarRol(Rol rolEditado, List<int> permisoIds)
        {
            if (rolEditado == null || rolEditado.Id <= 0 || string.IsNullOrWhiteSpace(rolEditado.Nombre))
            {
                throw new ArgumentException("El nombre del rol es obligatorio y el ID debe ser válido");
            }

            rolEditado.Nombre = rolEditado.Nombre.Trim();
            rolEditado.Estado = true;

            bool resultado = rol.EditarRol(rolEditado);
            if (resultado)
            {
                rolPermiso.ActualizarRolPermisos(rolEditado.Id, permisoIds);
            }

            return resultado;
        }

        public bool EliminarRol(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID del rol es inválido");
            }
            return rol.EliminarRol(id);
        }
    }
}
