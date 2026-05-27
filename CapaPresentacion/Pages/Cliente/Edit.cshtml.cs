using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System;

namespace CapaPresentacion.Pages.Cliente
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ClienteBLL clienteBll = new ClienteBLL();

        [BindProperty]
        public Entidades.Cliente ClienteEditado { get; set; } = new Entidades.Cliente();

        public IActionResult OnGet(int id)
        {
            try
            {
                ClienteEditado = clienteBll.ObtenerPorId(id);
                if (ClienteEditado == null)
                {
                    TempData["Swal_Message"] = "El cliente no existe";
                    TempData["Swal_Icon"] = "error";
                    return RedirectToPage("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Swal_Message"] = ex.Message;
                TempData["Swal_Icon"] = "error";
                return RedirectToPage("Index");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                var resultado = clienteBll.Editar(ClienteEditado);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Cliente actualizado exitosamente";
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
