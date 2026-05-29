using CapaNegocio;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace CapaPresentacion.Pages.Compra
{
    public class DetalleModel : PageModel
    {
        private readonly CompraBLL compraBLL = new CompraBLL();
        public Entidades.Compra CompraCabecera { get; set; }
        public List<DetalleCompra> Detalles { get; set; } = new List<DetalleCompra>();
        [TempData] public string MensajeError { get; set; }

        public IActionResult OnGet(int id)
        {
            CompraCabecera = compraBLL.ObtenerPorId(id);
            Detalles = compraBLL.ObtenerDetalles(id);
            return Page();
        }
    }
}
