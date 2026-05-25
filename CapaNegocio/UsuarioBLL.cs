using System;
using System.Collections.Generic;
using System.Text;
using CapaDatos;
using Entidades;

namespace CapaNegocio
{
    public class UsuarioBLL
    {
        private readonly UsuarioDAL usuario = new UsuarioDAL();
        public List<Usuario> ObtenerTodos() => usuario.ObtenerTodos();
        public Usuario ObtenerPorId(int id) => usuario.ObtenerPorId(id);

        public Usuario ValidarCredenciales(string Nombre, string contra)
        {
            // Validar parametros antes de consultar BD
            if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(contra))
            {
                return null;
            }

            //Buscar usuario en BD
            var Usuario = usuario.ObtenerPorNombre(Nombre);
            if (Usuario == null)
                return null;

            //Verificar hash de contraseña con BCrypt
            bool ContraValida = BCrypt.Net.BCrypt.Verify(contra, Usuario.Contra);

            //Si la contraseña no coincide, retorna null
            if (!ContraValida)
                return null;

            return Usuario;
        }

        public bool CrearUsuario(Usuario nuevoUsuario)
        {
            if (string.IsNullOrWhiteSpace(nuevoUsuario.Nombre))
            {
                throw new ArgumentException("El nombre de usuario es obligatorio");
            }

            // Validar longitud mínima del nombre
            if (nuevoUsuario.Nombre.Trim().Length < 3)
            {
                throw new ArgumentException("El nombre de usuario debe tener al menos 3 caracteres");
            }

            // Validar que la contraseña no esté vacía
            if (string.IsNullOrWhiteSpace(nuevoUsuario.Contra))
            {
                throw new ArgumentException("La contraseña es obligatoria");
            }

            // Validar longitud mínima de la contraseña (3 caracteres)
            if (nuevoUsuario.Contra.Length < 3)
            {
                throw new ArgumentException("La contraseña debe tener al menos 3 caracteres");
            }

            // Validar que se haya seleccionado un rol válido
            if (nuevoUsuario.RolId <= 0)
            {
                throw new ArgumentException("Debe seleccionar un rol válido");
            }

            // Verificar si el usuario ya existe
            var UsuarioExistente = usuario.ObtenerPorNombre(nuevoUsuario.Nombre.Trim());
            if (UsuarioExistente != null)
            {
                throw new InvalidOperationException("El nombre de usuario ya existe");
            }

            // Hash de contraseña con BCrypt
            nuevoUsuario.Contra = BCrypt.Net.BCrypt.HashPassword(nuevoUsuario.Contra);
            nuevoUsuario.Nombre = nuevoUsuario.Nombre.Trim();

            return usuario.CrearUsuario(nuevoUsuario);
        }

        public bool EditarUsuario(Usuario usuarioEditado)
        {
            // Validar que el ID sea válido
            if (usuarioEditado.Id <= 0)
            {
                throw new ArgumentException("El ID del usuario es inválido");
            }

            // Validar que el nombre no esté vacío
            if (string.IsNullOrWhiteSpace(usuarioEditado.Nombre))
            {
                throw new ArgumentException("El nombre de usuario es obligatorio");
            }

            // Validar longitud mínima del nombre
            if (usuarioEditado.Nombre.Trim().Length < 3)
            {
                throw new ArgumentException("El nombre de usuario debe tener al menos 3 caracteres");
            }

            // Validar que la contraseña sea minimo 3 caracteres
            if (!string.IsNullOrWhiteSpace(usuarioEditado.Contra) && usuarioEditado.Contra.Length < 3)
            {
                throw new ArgumentException("La contraseña debe tener al menos 3 caracteres");
            }

            // Validar que se haya seleccionado un rol válido
            if (usuarioEditado.RolId <= 0)
            {
                throw new ArgumentException("Debe seleccionar un rol válido");
            }

            // Verificar que el nombre no exista en otro usuario
            var usuarioExistente = usuario.ObtenerPorNombre(usuarioEditado.Nombre.Trim());
            if (usuarioExistente != null && usuarioExistente.Id != usuarioEditado.Id)
            {
                throw new InvalidOperationException("El nombre de usuario ya está en uso por otro usuario");
            }

            // Obtener el usuario actual para verificar existencia y conservar contraseña si es necesario
            var usuarioActual = usuario.ObtenerPorId(usuarioEditado.Id);
            if (usuarioActual == null)
            {
                throw new InvalidOperationException("El usuario no existe");
            }

            usuarioEditado.Nombre = usuarioEditado.Nombre.Trim();

            if (string.IsNullOrWhiteSpace(usuarioEditado.Contra))
            {
                // Conservar la contraseña encriptada actual
                usuarioEditado.Contra = usuarioActual.Contra;
            }
            else
            {
                // Encriptar la nueva contraseña
                usuarioEditado.Contra = BCrypt.Net.BCrypt.HashPassword(usuarioEditado.Contra);
            }

            return usuario.EditarUsuario(usuarioEditado);
        }

        public bool EliminarUsuario(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("El ID del usuario es inválido");
            }

            return usuario.EliminarUsuario(id);
        }
    }
}
