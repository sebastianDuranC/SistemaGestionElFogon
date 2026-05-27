using Dapper;
using Entidades;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaDatos
{
    public class MetodoPagoDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public List<MetodoPago> ObtenerTodos()
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<MetodoPago>(
                "sp_ListarMetodosPago",
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public MetodoPago ObtenerPorId(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            return Conexion.QueryFirstOrDefault<MetodoPago>(
                "sp_ObtenerMetodoPagoPorId",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public bool Crear(MetodoPago metodoPago)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_CrearMetodoPago",
                new { metodoPago.Nombre },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool Editar(MetodoPago metodoPago)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EditarMetodoPago",
                new { metodoPago.Id, metodoPago.Nombre },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool Eliminar(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EliminarMetodoPago",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }
    }
}
