using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Collections.Generic;
using System;

namespace CapaPresentacion.Pages.Proveedor
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ProveedorBLL proveedorBll = new ProveedorBLL();

        public List<Entidades.Proveedor> ListaProveedores { get; set; } = new List<Entidades.Proveedor>();

        public void OnGet()
        {
            ListaProveedores = proveedorBll.ObtenerTodos();
        }

        public IActionResult OnPostDelete(int id)
        {
            try
            {
                var resultado = proveedorBll.EliminarProveedor(id);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Proveedor eliminado exitosamente";
                    TempData["Swal_Icon"] = "success";
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo eliminar el proveedor";
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
