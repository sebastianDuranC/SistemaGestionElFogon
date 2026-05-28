using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Security.Claims;
using System;

namespace CapaPresentacion.Pages.Gastos
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly GastoOperativoBLL gastoBll = new GastoOperativoBLL();

        [BindProperty]
        public GastoOperativo NuevoGasto { get; set; } = new GastoOperativo();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            try
            {
                var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(claimId))
                    throw new Exception("Sesión inválida o expirada");

                NuevoGasto.UsuarioId = int.Parse(claimId);
                NuevoGasto.Estado = true;

                var resultado = gastoBll.Crear(NuevoGasto);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Gasto operativo registrado exitosamente";
                    TempData["Swal_Icon"] = "success";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo registrar el gasto operativo";
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
