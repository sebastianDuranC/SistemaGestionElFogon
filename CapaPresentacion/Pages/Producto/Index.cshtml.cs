using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Collections.Generic;
using System;

namespace CapaPresentacion.Pages.Producto
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ProductoBLL productoBll = new ProductoBLL();

        public List<Entidades.Producto> ListaProductos { get; set; } = new List<Entidades.Producto>();

        public void OnGet()
        {
            ListaProductos = productoBll.ObtenerTodos();
        }

        public IActionResult OnPostDelete(int id)
        {
            try
            {
                var resultado = productoBll.Eliminar(id);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Producto eliminado exitosamente";
                    TempData["Swal_Icon"] = "success";
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo eliminar el producto";
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
