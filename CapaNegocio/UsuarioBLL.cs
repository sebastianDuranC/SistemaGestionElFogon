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
        // Valida credenciales: usuario existe + contraseña correcta con BCrypt
        public Usuario ValidarCredenciales(string Nombre, string contra)
        {
            //Buscar usuario en BD
            var Usuario = usuario.ObtenerPorNombre(Nombre);
            if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(contra))
            {
                return null;
            }

            if (Usuario == null)
                return null;

            //Verificar hash de contraseña con BCrypt
            bool ContraValida = BCrypt.Net.BCrypt.Verify(contra, Usuario.Contra);

            //Si la contraseña no coincide, retorna null
            if (!ContraValida)
                return null;

            return Usuario;
        }
    }
}
