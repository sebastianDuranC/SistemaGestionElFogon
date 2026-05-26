using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System;

namespace CapaPresentacion.Pages.ProductoCategoria
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ProductoCategoriaBLL productoCategoriaBll = new ProductoCategoriaBLL();

        [BindProperty]
        public Entidades.ProductoCategoria CategoriaEditada { get; set; } = new Entidades.ProductoCategoria();

        public IActionResult OnGet(int id)
        {
            CategoriaEditada = productoCategoriaBll.ObtenerPorId(id);
            if (CategoriaEditada == null)
            {
                TempData["Swal_Message"] = "La categoría de producto no existe";
                TempData["Swal_Icon"] = "error";
                return RedirectToPage("Index");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                var resultado = productoCategoriaBll.Editar(CategoriaEditada);
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
