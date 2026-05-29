using CapaDatos;
using CapaNegocio;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;

namespace CapaPresentacion.Pages.Compra
{
    public class IndexModel : PageModel
    {
        CompraBLL compraBLL = new ();
        public List<Entidades.Compra> Compras { get; set; } = new List<Entidades.Compra>();
        public void OnGet()
        {
            Compras = compraBLL.Listar();
        }

        public IActionResult OnPostAnular(int id)
        {
            try
            {
                int usuarioClaimId = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                var resultado = compraBLL.Anular(id, usuarioClaimId);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Compra anulada exitosamente";
                    TempData["Swal_Icon"] = "success";
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo anular la compra";
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
