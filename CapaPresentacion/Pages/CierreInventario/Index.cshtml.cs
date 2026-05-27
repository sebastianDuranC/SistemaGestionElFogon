using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Collections.Generic;

namespace CapaPresentacion.Pages.CierreInventario
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly CierreInventarioBLL cierreBll = new CierreInventarioBLL();

        public List<Entidades.CierreInventario> ListaCierres { get; set; } = new List<Entidades.CierreInventario>();

        public void OnGet()
        {
            ListaCierres = cierreBll.ObtenerTodos();
        }
    }
}
