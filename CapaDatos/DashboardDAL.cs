using Dapper;
using Entidades;
using System.Data;
using System.Linq;

namespace CapaDatos
{
    public class DashboardDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public DashboardResumen ObtenerResumenDashboard()
        {
            var dashboardResumen = new DashboardResumen();
            using var dbConnection = conexion.ObtenerConexion();
            
            //usamos dapper querymultiple para leer multiples resultados en un solo viaje
            using var multi = dbConnection.QueryMultiple(
                "sp_ObtenerDatosDashboard",
                commandType: CommandType.StoredProcedure
            );

            //mapeo directo de las metricas principales de las tarjetas
            var metricas = multi.Read<DashboardResumen>().FirstOrDefault();
            if (metricas != null)
            {
                dashboardResumen.TotalVentasHoy = metricas.TotalVentasHoy;
                dashboardResumen.VentasMesBs = metricas.VentasMesBs;
                dashboardResumen.TotalInsumos = metricas.TotalInsumos;
                dashboardResumen.InsumosStockBajoCount = metricas.InsumosStockBajoCount;
            }

            //mapeo de la lista de productos mas vendidos
            dashboardResumen.ProductosMasVendidos = multi.Read<ProductoMasVendido>().ToList();

            //mapeo de la lista de metodos de pago mas usados
            dashboardResumen.MetodosPagoMasUsados = multi.Read<MetodoPagoMasUsado>().ToList();

            return dashboardResumen;
        }
    }
}
