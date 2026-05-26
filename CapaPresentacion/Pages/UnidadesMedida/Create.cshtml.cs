using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System;

namespace CapaPresentacion.Pages.UnidadesMedida
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly UnidadMedidaBLL unidadmedidaBll = new UnidadMedidaBLL();

        [BindProperty]
        public Entidades.UnidadMedida NuevaUnidad { get; set; } = new Entidades.UnidadMedida();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            try
            {
                NuevaUnidad.Estado = true;
                var resultado = unidadmedidaBll.Crear(NuevaUnidad);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Unidad de medida creada exitosamente";
                    TempData["Swal_Icon"] = "success";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo registrar la unidad";
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
