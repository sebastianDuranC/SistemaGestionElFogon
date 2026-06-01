using CapaNegocio;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace CapaPresentacion.Pages.Venta
{
    public class DetalleModel : PageModel
    {
        private readonly VentaBLL ventaBLL = new VentaBLL();
        private readonly ClienteBLL clienteBLL = new ClienteBLL();

        public Entidades.Venta VentaCabecera { get; set; }
        public List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
        public List<DetallePago> Pagos { get; set; } = new List<DetallePago>();
        public Entidades.Cliente ClienteInfo { get; set; }

        public IActionResult OnGet(int id)
        {
            VentaCabecera = ventaBLL.ObtenerPorId(id);
            if (VentaCabecera == null)
            {
                TempData["Swal_Message"] = "No se encontró la venta solicitada.";
                TempData["Swal_Icon"] = "error";
                return RedirectToPage("./Index");
            }

            Detalles = ventaBLL.ObtenerDetallesVenta(id);
            Pagos = ventaBLL.ObtenerDetallesPago(id);

            if (VentaCabecera.ClienteId.HasValue && VentaCabecera.ClienteId.Value > 0)
            {
                ClienteInfo = clienteBLL.ObtenerPorId(VentaCabecera.ClienteId.Value);
            }

            return Page();
        }
    }
}
