using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Collections.Generic;
using System;

namespace CapaPresentacion.Pages.InsumoCategoria
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly InsumoCategoriaBLL insumoCategoriaBll = new InsumoCategoriaBLL();

        public List<Entidades.InsumoCategoria> ListaCategorias { get; set; } = new List<Entidades.InsumoCategoria>();

        public void OnGet()
        {
            ListaCategorias = insumoCategoriaBll.ObtenerTodos();
        }

        public IActionResult OnPostDelete(int id)
        {
            try
            {
                var resultado = insumoCategoriaBll.Eliminar(id);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Categoría de insumo eliminada exitosamente";
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
