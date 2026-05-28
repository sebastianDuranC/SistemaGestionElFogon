using Dapper;
using Entidades;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaDatos
{
    public class GastoOperativoDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public List<GastoOperativo> ObtenerTodos()
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<GastoOperativo>(
                "sp_ListarGastosOperativos",
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public GastoOperativo ObtenerPorId(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            return Conexion.QueryFirstOrDefault<GastoOperativo>(
                "sp_ObtenerGastoOperativoPorId",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public bool Crear(GastoOperativo gasto)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_CrearGastoOperativo",
                new { gasto.Concepto, gasto.Monto, gasto.UsuarioId },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool Editar(GastoOperativo gasto)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EditarGastoOperativo",
                new { gasto.Id, gasto.Concepto, gasto.Monto },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool Eliminar(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EliminarGastoOperativo",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }
    }
}
