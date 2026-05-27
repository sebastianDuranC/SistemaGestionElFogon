using Dapper;
using Entidades;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaDatos
{
    public class ControlCajaResumen
    {
        public decimal MontoApertura { get; set; }
        public decimal VentasEfectivo { get; set; }
        public decimal TotalEgresos { get; set; }
        public decimal MontoCierreEsperado { get; set; }
    }

    public class ControlCajaDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public ControlCaja ObtenerCajaActiva()
        {
            using var Conexion = conexion.ObtenerConexion();
            return Conexion.QueryFirstOrDefault<ControlCaja>(
                "sp_ObtenerEstadoCajaActual",
                commandType: CommandType.StoredProcedure
            );
        }

        public List<ControlCaja> ObtenerHistorico()
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<ControlCaja>(
                "sp_ListarControlCajaHistorico",
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public bool AbrirCaja(ControlCaja cc)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_AbrirCaja",
                new
                {
                    cc.MontoApertura,
                    cc.UsuarioId,
                    cc.NegocioId
                },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool CerrarCaja(ControlCaja cc)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_CerrarCaja",
                new
                {
                    cc.Id,
                    cc.MontoCierreEsperado,
                    cc.MontoCierreReal,
                    cc.Diferencial
                },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public ControlCajaResumen ObtenerResumenCaja(int controlCajaId)
        {
            using var Conexion = conexion.ObtenerConexion();
            return Conexion.QueryFirstOrDefault<ControlCajaResumen>(
                "sp_ObtenerResumenCaja",
                new { ControlCajaId = controlCajaId },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
