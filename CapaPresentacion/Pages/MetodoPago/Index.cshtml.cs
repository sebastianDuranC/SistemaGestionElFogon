using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Collections.Generic;
using System;

namespace CapaPresentacion.Pages.MetodoPago
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly MetodoPagoBLL metodoPagoBll = new MetodoPagoBLL();

        public List<Entidades.MetodoPago> ListaMetodosPago { get; set; } = new List<Entidades.MetodoPago>();

        public void OnGet()
        {
            ListaMetodosPago = metodoPagoBll.ObtenerTodos();
        }

        public IActionResult OnPostDelete(int id)
        {
            try
            {
                var resultado = metodoPagoBll.Eliminar(id);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Método de pago eliminado exitosamente";
                    TempData["Swal_Icon"] = "success";
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo eliminar el método de pago";
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
    }
}
