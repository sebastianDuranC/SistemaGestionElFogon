using Entidades;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CapaDatos
{
    public class PermisoDAL
    {
        private readonly ConexionDAL _conexion = new ConexionDAL();

        public List<string> ObtenerRutasPermitidas(int RolId)
        {
            var Rutas = new List<string>();

            using (var Conexion = _conexion.ObtenerConexion())
            using (var Comando = new SqlCommand("sp_ObtenerRutasPermitidasPorRol", Conexion))
            {
                Comando.CommandType = CommandType.StoredProcedure;
                Comando.Parameters.AddWithValue("@RolId", RolId);

                Conexion.Open();

                using (var Lector = Comando.ExecuteReader())
                {
                    while (Lector.Read())
                    {
                        Rutas.Add(Lector["FormRuta"].ToString());
                    }
                }
            }

            return Rutas;
        }

        public List<Permiso> ObtenerTodos()
        {
            var Lista = new List<Permiso>();

            using (var Conexion = _conexion.ObtenerConexion())
            using (var Comando = new SqlCommand("sp_ListarPermisos", Conexion))
            {
                Comando.CommandType = CommandType.StoredProcedure;
                Conexion.Open();

                using (var Lector = Comando.ExecuteReader())
                {
                    while (Lector.Read())
                    {
                        Lista.Add(new Permiso
                        {
                            Id = Convert.ToInt32(Lector["Id"]),
                            FormNombre = Lector["FormNombre"].ToString(),
                            FormRuta = Lector["FormRuta"].ToString(),
                            Modulo = Lector["Modulo"].ToString(),
                            Estado = Convert.ToBoolean(Lector["Estado"])
                        });
                    }
                }
            }

            return Lista;
        }
    }
}
