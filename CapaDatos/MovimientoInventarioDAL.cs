using Dapper;
using Entidades;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaDatos
{
    public class MovimientoInventarioDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public List<MovimientoInventario> ObtenerTodos()
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<MovimientoInventario>(
                "sp_ListarMovimientosInventario",
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public bool Registrar(MovimientoInventario movimiento)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_RegistrarMovimientoInventario",
                new
                {
                    movimiento.InsumoId,
                    movimiento.TipoMovimiento,
                    movimiento.Cantidad,
                    movimiento.Observacion,
                    movimiento.UsuarioId
                },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }
    }
}
