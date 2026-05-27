using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Collections.Generic;

namespace CapaPresentacion.Pages.MovimientoInventario
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly MovimientoInventarioBLL movimientoBll = new MovimientoInventarioBLL();

        public List<Entidades.MovimientoInventario> ListaMovimientos { get; set; } = new List<Entidades.MovimientoInventario>();

        public void OnGet()
        {
            ListaMovimientos = movimientoBll.ObtenerTodos();
        }
    }
}
