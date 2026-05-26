using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Collections.Generic;
using System;

namespace CapaPresentacion.Pages.UnidadesMedida
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly UnidadMedidaBLL unidadmedidaBll = new UnidadMedidaBLL();

        public List<Entidades.UnidadMedida> ListaUnidades { get; set; } = new List<Entidades.UnidadMedida>();

        public void OnGet()
        {
            ListaUnidades = unidadmedidaBll.ObtenerTodos();
        }

        public IActionResult OnPostDelete(int id)
        {
            try
            {
                var resultado = unidadmedidaBll.Eliminar(id);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Unidad de medida eliminada exitosamente";
                    TempData["Swal_Icon"] = "success";
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo eliminar la unidad de medida";
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
