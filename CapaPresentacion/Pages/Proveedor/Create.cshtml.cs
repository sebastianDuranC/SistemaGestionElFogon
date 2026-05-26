using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System;

namespace CapaPresentacion.Pages.Proveedor
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ProveedorBLL proveedorBll = new ProveedorBLL();

        [BindProperty]
        public Entidades.Proveedor NuevoProveedor { get; set; } = new Entidades.Proveedor();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            try
            {
                NuevoProveedor.Estado = true;
                var resultado = proveedorBll.CrearProveedor(NuevoProveedor);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Proveedor registrado exitosamente";
                    TempData["Swal_Icon"] = "success";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo registrar el proveedor";
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
