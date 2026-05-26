using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Collections.Generic;
using System;

namespace CapaPresentacion.Pages.ProductoCategoria
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ProductoCategoriaBLL productoCategoriaBll = new ProductoCategoriaBLL();

        public List<Entidades.ProductoCategoria> ListaCategorias { get; set; } = new List<Entidades.ProductoCategoria>();

        public void OnGet()
        {
            ListaCategorias = productoCategoriaBll.ObtenerTodos();
        }

        public IActionResult OnPostDelete(int id)
        {
            try
            {
                var resultado = productoCategoriaBll.Eliminar(id);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Categoría eliminada exitosamente";
                    TempData["Swal_Icon"] = "success";
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo eliminar la categoría";
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
