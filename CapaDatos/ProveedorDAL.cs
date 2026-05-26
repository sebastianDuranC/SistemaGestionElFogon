using Dapper;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaDatos
{
    public class ProveedorDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public List<Proveedor> ObtenerTodos()
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<Proveedor>(
                "sp_ListarProveedores",
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public Proveedor ObtenerPorId(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            return Conexion.QueryFirstOrDefault<Proveedor>(
                "sp_ObtenerProveedorPorId",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public bool CrearProveedor(Proveedor proveedor)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_CrearProveedor",
                new
                {
                    proveedor.Nombre,
                    proveedor.Apellido,
                    proveedor.Contacto
                },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool EditarProveedor(Proveedor proveedor)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EditarProveedor",
                new
                {
                    proveedor.Id,
                    proveedor.Nombre,
                    proveedor.Apellido,
                    proveedor.Contacto
                },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool EliminarProveedor(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EliminarProveedor",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }
    }
}
