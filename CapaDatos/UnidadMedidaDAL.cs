using Dapper;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaDatos
{
    public class UnidadMedidaDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public List<UnidadMedida> ObtenerTodos()
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<UnidadMedida>(
                "sp_ListarUnidadesMedida",
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public UnidadMedida ObtenerPorId(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            return Conexion.QueryFirstOrDefault<UnidadMedida>(
                "sp_ObtenerUnidadMedidaPorId",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public bool Crear(UnidadMedida unidad)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_CrearUnidadMedida",
                new { unidad.Nombre, unidad.Abreviatura },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool Editar(UnidadMedida unidad)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EditarUnidadMedida",
                new { unidad.Id, unidad.Nombre, unidad.Abreviatura },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool Eliminar(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EliminarUnidadMedida",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }
    }
}
