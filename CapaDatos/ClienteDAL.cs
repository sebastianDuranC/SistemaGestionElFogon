using Dapper;
using Entidades;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaDatos
{
    public class ClienteDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public List<Cliente> ObtenerTodos()
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<Cliente>(
                "sp_ListarClientes",
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public Cliente ObtenerPorId(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            return Conexion.QueryFirstOrDefault<Cliente>(
                "sp_ObtenerClientePorId",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public bool Crear(Cliente cliente)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_CrearCliente",
                new
                {
                    cliente.Nombre,
                    cliente.Apellido,
                    cliente.EsComerciante,
                    cliente.NumeroLocal,
                    cliente.Pasillo
                },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool Editar(Cliente cliente)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EditarCliente",
                new
                {
                    cliente.Id,
                    cliente.Nombre,
                    cliente.Apellido,
                    cliente.EsComerciante,
                    cliente.NumeroLocal,
                    cliente.Pasillo
                },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool Eliminar(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EliminarCliente",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }
    }
}
