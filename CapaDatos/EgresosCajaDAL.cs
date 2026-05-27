using Dapper;
using Entidades;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaDatos
{
    public class EgresosCajaDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public List<EgresosCaja> ObtenerEgresosPorCaja(int? controlCajaId = null)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<EgresosCaja>(
                "sp_ListarEgresosCaja",
                new { ControlCajaId = controlCajaId },
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public bool Registrar(EgresosCaja egreso)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_RegistrarEgresoCaja",
                new
                {
                    egreso.Motivo,
                    egreso.Monto,
                    egreso.ControlCajaId,
                    egreso.UsuarioId
                },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }
    }
}
