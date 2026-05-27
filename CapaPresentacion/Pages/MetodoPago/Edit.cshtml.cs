using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System;

namespace CapaPresentacion.Pages.MetodoPago
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly MetodoPagoBLL metodoPagoBll = new MetodoPagoBLL();

        [BindProperty]
        public Entidades.MetodoPago MetodoPagoEditado { get; set; } = new Entidades.MetodoPago();

        public IActionResult OnGet(int id)
        {
            try
            {
                MetodoPagoEditado = metodoPagoBll.ObtenerPorId(id);
                if (MetodoPagoEditado == null)
                {
                    TempData["Swal_Message"] = "El método de pago no existe";
                    TempData["Swal_Icon"] = "error";
                    return RedirectToPage("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Swal_Message"] = ex.Message;
                TempData["Swal_Icon"] = "error";
                return RedirectToPage("Index");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                var resultado = metodoPagoBll.Editar(MetodoPagoEditado);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Método de pago actualizado exitosamente";
                    TempData["Swal_Icon"] = "success";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudieron guardar los cambios";
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
