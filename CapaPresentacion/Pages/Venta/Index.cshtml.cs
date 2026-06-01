using CapaNegocio;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace CapaPresentacion.Pages.Venta
{
    public class IndexModel : PageModel
    {
        private readonly VentaBLL ventaBLL = new VentaBLL();
        public List<Entidades.Venta> Ventas { get; set; } = new List<Entidades.Venta>();

        public void OnGet()
        {
            Ventas = ventaBLL.Listar();
        }

        public IActionResult OnPostAnular(int id)
        {
            try
            {
                int usuarioClaimId = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                var resultado = ventaBLL.Anular(id, usuarioClaimId);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Venta anulada correctamente";
                    TempData["Swal_Icon"] = "success";
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo anular la venta";
                    TempData["Swal_Icon"] = "error";
                }
            }
            catch (Exception ex)
            {
                TempData["Swal_Message"] = ex.Message;
                TempData["Swal_Icon"] = "error";
            }
            return RedirectToPage();
        }

        public IActionResult OnPostDevolverPlatos(int id)
        {
            try
            {
                var resultado = ventaBLL.DevolverPlatos(id);
                if (resultado)
                {
                    return new JsonResult(new { success = true, message = "Platos marcados como devueltos." });
                }
                return new JsonResult(new { success = false, message = "No se pudo actualizar el estado de los platos." });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }
    }
}
