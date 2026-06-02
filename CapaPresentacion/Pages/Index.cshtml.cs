using CapaNegocio;
using Entidades;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CapaPresentacion.Pages
{
    public class IndexModel : PageModel
    {
        private readonly DashboardBLL dashboardBLL = new DashboardBLL();

        //resumen del panel expuesto a la vista
        public DashboardResumen Resumen { get; set; } = new();

        public void OnGet()
        {
            //cargar el resumen desde el bll
            Resumen = dashboardBLL.ObtenerResumenDashboard();
        }
    }
}
