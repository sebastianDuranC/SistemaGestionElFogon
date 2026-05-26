using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Collections.Generic;
using System;

namespace CapaPresentacion.Pages.Insumo
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly InsumoBLL insumoBll = new InsumoBLL();

        public List<Entidades.Insumo> ListaInsumos { get; set; } = new List<Entidades.Insumo>();

        public void OnGet()
        {
            ListaInsumos = insumoBll.ObtenerTodos();
        }

        public IActionResult OnPostDelete(int id)
        {
            try
            {
                var resultado = insumoBll.Eliminar(id);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Insumo eliminado exitosamente";
                    TempData["Swal_Icon"] = "success";
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo eliminar el insumo";
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
