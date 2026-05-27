using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Collections.Generic;

namespace CapaPresentacion.Pages.EgresoCaja
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly EgresosCajaBLL egresosBll = new EgresosCajaBLL();

        public List<Entidades.EgresosCaja> ListaEgresos { get; set; } = new List<Entidades.EgresosCaja>();

        public void OnGet()
        {
            ListaEgresos = egresosBll.ObtenerEgresosPorCaja();
        }
    }
}
