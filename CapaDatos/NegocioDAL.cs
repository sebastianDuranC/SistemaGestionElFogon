using Dapper;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CapaDatos
{
    public class NegocioDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public Negocio ObtenerDatosNegocio()
        {
            using var Conexion = conexion.ObtenerConexion();
            return Conexion.QueryFirstOrDefault<Negocio>(
                "sp_ListarNegocio",
                commandType: CommandType.StoredProcedure
            );
        }

        public Negocio ObtenerDatosNegocioPorId(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            return Conexion.QueryFirstOrDefault<Negocio>(
                "sp_ObtenerNegocioPorId",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public bool EditarDatosNegocio(Negocio negocio) 
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute
            (
                "sp_EditarNegocio",
                new
                {
                    negocio.Id,
                    negocio.Nombre,
                    negocio.Direccion,
                    negocio.LogoUrl,
                },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }
    }
}
