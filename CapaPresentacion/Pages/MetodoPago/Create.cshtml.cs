using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System;

namespace CapaPresentacion.Pages.MetodoPago
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly MetodoPagoBLL metodoPagoBll = new MetodoPagoBLL();

        [BindProperty]
        public Entidades.MetodoPago NuevoMetodoPago { get; set; } = new Entidades.MetodoPago();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            try
            {
                NuevoMetodoPago.Estado = true;
                var resultado = metodoPagoBll.Crear(NuevoMetodoPago);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Método de pago registrado exitosamente";
                    TempData["Swal_Icon"] = "success";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo registrar el método de pago";
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
