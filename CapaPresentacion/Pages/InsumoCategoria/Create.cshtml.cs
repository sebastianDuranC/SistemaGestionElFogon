using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System;

namespace CapaPresentacion.Pages.InsumoCategoria
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly InsumoCategoriaBLL insumoCategoriaBll = new InsumoCategoriaBLL();

        [BindProperty]
        public Entidades.InsumoCategoria NuevaCategoria { get; set; } = new Entidades.InsumoCategoria();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            try
            {
                NuevaCategoria.Estado = true;
                var resultado = insumoCategoriaBll.Crear(NuevaCategoria);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Categoría creada exitosamente";
                    TempData["Swal_Icon"] = "success";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo registrar la categoría";
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
