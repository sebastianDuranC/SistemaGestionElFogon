using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Collections.Generic;
using System.Security.Claims;
using System;

namespace CapaPresentacion.Pages.Gastos
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly GastoOperativoBLL gastoBll = new GastoOperativoBLL();

        public List<GastoOperativo> ListaGastos { get; set; } = new List<GastoOperativo>();

        public void OnGet()
        {
            ListaGastos = gastoBll.ObtenerTodos();
        }

        public IActionResult OnPostDelete(int id)
        {
            try
            {
                var resultado = gastoBll.Eliminar(id);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Gasto operativo eliminado exitosamente";
                    TempData["Swal_Icon"] = "success";
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo eliminar el gasto operativo";
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
