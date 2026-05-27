using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Security.Claims;
using System;

namespace CapaPresentacion.Pages.ControlCaja
{
    [Authorize]
    public class AperturaModel : PageModel
    {
        private readonly ControlCajaBLL controlCajaBll = new ControlCajaBLL();

        [BindProperty]
        public Entidades.ControlCaja NuevoTurno { get; set; } = new Entidades.ControlCaja();

        public IActionResult OnGet()
        {
            // Si ya hay una caja abierta, no permitir apertura y redirigir
            var cajaActiva = controlCajaBll.ObtenerCajaActiva();
            if (cajaActiva != null)
            {
                TempData["Swal_Message"] = "La caja ya está abierta";
                TempData["Swal_Icon"] = "warning";
                return RedirectToPage("Index");
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

                NuevoTurno.UsuarioId = int.Parse(claimId);
                NuevoTurno.NegocioId = 1;
                NuevoTurno.Estado = true;

                var resultado = controlCajaBll.AbrirCaja(NuevoTurno);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Caja abierta correctamente. ¡Buen turno!";
                    TempData["Swal_Icon"] = "success";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo realizar la apertura de caja";
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
