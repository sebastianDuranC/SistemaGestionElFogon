using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Collections.Generic;
using System;

namespace CapaPresentacion.Pages.Cliente
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ClienteBLL clienteBll = new ClienteBLL();

        public List<Entidades.Cliente> ListaClientes { get; set; } = new List<Entidades.Cliente>();

        public void OnGet()
        {
            ListaClientes = clienteBll.ObtenerTodos();
        }

        public IActionResult OnPostDelete(int id)
        {
            try
            {
                var resultado = clienteBll.Eliminar(id);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Cliente eliminado exitosamente";
                    TempData["Swal_Icon"] = "success";
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo eliminar el cliente";
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
