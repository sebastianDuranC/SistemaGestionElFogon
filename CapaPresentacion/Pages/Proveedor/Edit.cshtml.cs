using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System;

namespace CapaPresentacion.Pages.Proveedor
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ProveedorBLL proveedorBll = new ProveedorBLL();

        [BindProperty]
        public Entidades.Proveedor ProveedorEditado { get; set; } = new Entidades.Proveedor();

        public IActionResult OnGet(int id)
        {
            ProveedorEditado = proveedorBll.ObtenerPorId(id);
            if (ProveedorEditado == null)
            {
                TempData["Swal_Message"] = "El proveedor no existe";
                TempData["Swal_Icon"] = "error";
                return RedirectToPage("Index");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                var resultado = proveedorBll.EditarProveedor(ProveedorEditado);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Proveedor modificado correctamente";
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
