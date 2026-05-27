using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Collections.Generic;
using System.Security.Claims;
using System;

namespace CapaPresentacion.Pages.MovimientoInventario
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly MovimientoInventarioBLL movimientoBll = new MovimientoInventarioBLL();
        private readonly InsumoBLL insumoBll = new InsumoBLL();

        [BindProperty]
        public Entidades.MovimientoInventario NuevoMovimiento { get; set; } = new Entidades.MovimientoInventario();

        public List<Entidades.Insumo> ListaInsumos { get; set; } = new List<Entidades.Insumo>();

        public void OnGet()
        {
            ListaInsumos = insumoBll.ObtenerTodos();
        }

        public IActionResult OnPost()
        {
            try
            {
                // Obtener ID del usuario logeado
                var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(claimId))
                    throw new Exception("Sesión inválida o expirada");

                NuevoMovimiento.UsuarioId = int.Parse(claimId);
                NuevoMovimiento.TipoMovimiento = "Merma"; // Se registra específicamente como merma/pérdida
                NuevoMovimiento.Estado = true;

                var resultado = movimientoBll.Registrar(NuevoMovimiento);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Merma registrada exitosamente";
                    TempData["Swal_Icon"] = "success";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo registrar la merma";
                    TempData["Swal_Icon"] = "error";
                }
            }
            catch (Exception ex)
            {
                TempData["Swal_Message"] = ex.Message;
                TempData["Swal_Icon"] = "error";
            }

            // Recargar lista en caso de error
            ListaInsumos = insumoBll.ObtenerTodos();
            return Page();
        }
    }
}
