using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapaDatos
{
    public class ConexionDAL
    {
        // Cadena estática que se configura desde Program.cs
            public static string CadenaConexion { get; set; }

        // Retorna una nueva conexión lista para usar
        public SqlConnection ObtenerConexion()
        {
            return new SqlConnection(CadenaConexion);
        }
    }
}
