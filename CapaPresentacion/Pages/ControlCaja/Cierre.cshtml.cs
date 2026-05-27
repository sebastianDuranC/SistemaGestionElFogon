using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using CapaDatos;
using System;

namespace CapaPresentacion.Pages.ControlCaja
{
    [Authorize]
    public class CierreModel : PageModel
    {
        private readonly ControlCajaBLL controlCajaBll = new ControlCajaBLL();

        public Entidades.ControlCaja CajaActiva { get; set; }
        public ControlCajaResumen Resumen { get; set; }

        [BindProperty]
        public decimal MontoCierreReal { get; set; }

        public IActionResult OnGet()
        {
            CajaActiva = controlCajaBll.ObtenerCajaActiva();
            if (CajaActiva == null)
            {
                TempData["Swal_Message"] = "No hay ninguna caja abierta actualmente";
                TempData["Swal_Icon"] = "warning";
                return RedirectToPage("Index");
            }
            Resumen = controlCajaBll.ObtenerResumenCaja(CajaActiva.Id);
            MontoCierreReal = Resumen.MontoCierreEsperado; // Inicializar con el esperado
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                var resultado = controlCajaBll.CerrarCaja(MontoCierreReal);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Caja cerrada correctamente. ¡Cierre completado!";
                    TempData["Swal_Icon"] = "success";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo realizar el cierre de caja";
                    TempData["Swal_Icon"] = "error";
                }
            }
            catch (Exception ex)
            {
                TempData["Swal_Message"] = ex.Message;
                TempData["Swal_Icon"] = "error";
            }

            // Recargar datos en caso de error
            CajaActiva = controlCajaBll.ObtenerCajaActiva();
            if (CajaActiva != null)
            {
                Resumen = controlCajaBll.ObtenerResumenCaja(CajaActiva.Id);
            }
            return Page();
        }
    }
}
