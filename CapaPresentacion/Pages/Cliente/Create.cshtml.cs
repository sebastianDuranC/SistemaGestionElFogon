using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System;

namespace CapaPresentacion.Pages.Cliente
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ClienteBLL clienteBll = new ClienteBLL();

        [BindProperty]
        public Entidades.Cliente NuevoCliente { get; set; } = new Entidades.Cliente();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            try
            {
                NuevoCliente.Estado = true;
                var resultado = clienteBll.Crear(NuevoCliente);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Cliente registrado exitosamente";
                    TempData["Swal_Icon"] = "success";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo registrar el cliente";
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
