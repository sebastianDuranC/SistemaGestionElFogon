using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Entidades;

namespace CapaDatos
{
    public class UsuarioDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public Usuario ObtenerPorNombre(string Nombre)
        {
            Usuario usuario = new Usuario();

            using (var Conexion = conexion.ObtenerConexion())
            using (var Comando = new SqlCommand("sp_ObtenerUsuarioPorNombre", Conexion))
            {
                Comando.CommandType = CommandType.StoredProcedure;
                Comando.Parameters.AddWithValue("@Nombre", Nombre);

                Conexion.Open();

                using (var Lector = Comando.ExecuteReader())
                {
                    if (Lector.Read())
                    {
                        usuario = new Usuario
                        {
                            Id = Convert.ToInt32(Lector["Id"]),
                            Nombre = Lector["Nombre"].ToString(),
                            Contra = Lector["Contra"].ToString(),
                            RolId = Convert.ToInt32(Lector["RolId"]),
                            NegocioId = Convert.ToInt32(Lector["NegocioId"]),
                            Estado = Convert.ToBoolean(Lector["Estado"])
                        };
                    }
                }
            }

            return usuario;
        }

        public List<Usuario> ObtenerTodos()
        {
            List<Usuario> Lista = new List<Usuario>();

            using (var Conexion = conexion.ObtenerConexion())
            using (var Comando = new SqlCommand("sp_ObtenerUsuarios", Conexion))
            {
                Comando.CommandType = CommandType.StoredProcedure;
                Conexion.Open();

                using (var Lector = Comando.ExecuteReader())
                {
                    while (Lector.Read())
                    {
                        Lista.Add(new Usuario
                        {
                            Id = Convert.ToInt32(Lector["Id"]),
                            Nombre = Lector["Nombre"].ToString(),
                            RolId = Convert.ToInt32(Lector["RolId"]),
                            NombreRol = Lector["NombreRol"].ToString(),
                            NegocioId = Convert.ToInt32(Lector["NegocioId"]),
                            Estado = Convert.ToBoolean(Lector["Estado"])
                        });
                    }
                }
            }

            return Lista;
        }
    }
}
