using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System;
using System.Collections.Generic;

namespace CapaPresentacion.Pages.Rol
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly RolBLL rolBLL = new RolBLL();
        public List<Entidades.Rol> ListaRoles { get; set; } = new List<Entidades.Rol>();

        public void OnGet()
        {
            ListaRoles = rolBLL.ObtenerTodos();
        }

        public IActionResult OnPostDelete(int id)
        {
            try
            {
                var resultado = rolBLL.EliminarRol(id);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Rol eliminado exitosamente";
                    TempData["Swal_Icon"] = "success";
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo eliminar el rol";
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
