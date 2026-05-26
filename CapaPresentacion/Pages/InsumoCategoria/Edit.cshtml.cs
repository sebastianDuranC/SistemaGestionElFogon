using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System;

namespace CapaPresentacion.Pages.InsumoCategoria
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly InsumoCategoriaBLL insumoCategoriaBll = new InsumoCategoriaBLL();

        [BindProperty]
        public Entidades.InsumoCategoria CategoriaEditada { get; set; } = new Entidades.InsumoCategoria();

        public IActionResult OnGet(int id)
        {
            CategoriaEditada = insumoCategoriaBll.ObtenerPorId(id);
            if (CategoriaEditada == null)
            {
                TempData["Swal_Message"] = "La categoría de insumo no existe";
                TempData["Swal_Icon"] = "error";
                return RedirectToPage("Index");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                var resultado = insumoCategoriaBll.Editar(CategoriaEditada);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Categoría modificada correctamente";
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
