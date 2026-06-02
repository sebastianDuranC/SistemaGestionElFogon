using CapaDatos;
using Entidades;

namespace CapaNegocio
{
    public class DashboardBLL
    {
        private readonly DashboardDAL dashboardDAL = new DashboardDAL();

        public DashboardResumen ObtenerResumenDashboard()
        {
            //retorna el resumen directamente desde el acceso a datos
            return dashboardDAL.ObtenerResumenDashboard();
        }
    }
}
