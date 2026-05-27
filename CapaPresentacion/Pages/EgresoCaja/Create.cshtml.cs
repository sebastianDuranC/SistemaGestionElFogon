using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Security.Claims;
using System;

namespace CapaPresentacion.Pages.EgresoCaja
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly EgresosCajaBLL egresosBll = new EgresosCajaBLL();
        private readonly ControlCajaBLL controlCajaBll = new ControlCajaBLL();

        [BindProperty]
        public Entidades.EgresosCaja NuevoEgreso { get; set; } = new Entidades.EgresosCaja();

        public IActionResult OnGet()
        {
            // Validar que la caja esté abierta para poder registrar egresos
            var cajaActiva = controlCajaBll.ObtenerCajaActiva();
            if (cajaActiva == null)
            {
                TempData["Swal_Message"] = "Debe abrir la caja antes de poder registrar un egreso";
                TempData["Swal_Icon"] = "warning";
                return RedirectToPage("/ControlCaja/Index");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(claimId))
                    throw new Exception("Sesión inválida o expirada");

                NuevoEgreso.UsuarioId = int.Parse(claimId);
                NuevoEgreso.Estado = true;

                var resultado = egresosBll.Registrar(NuevoEgreso);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Egreso de caja registrado exitosamente";
                    TempData["Swal_Icon"] = "success";
                    return RedirectToPage("/ControlCaja/Index");
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo registrar el egreso de caja";
                    TempData["Swal_Icon"] = "error";
                }
            }
            catch (Exception ex)
            {
                TempData["Swal_Message"] = ex.Message;
                TempData["Swal_Icon"] = "error";
            }
            return Page();
        }
    }
}
