using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System;

namespace CapaPresentacion.Pages.Gastos
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly GastoOperativoBLL gastoBll = new GastoOperativoBLL();

        [BindProperty]
        public GastoOperativo GastoEditado { get; set; } = new GastoOperativo();

        public IActionResult OnGet(int id)
        {
            try
            {
                GastoEditado = gastoBll.ObtenerPorId(id);
                if (GastoEditado == null)
                {
                    TempData["Swal_Message"] = "El gasto operativo no existe";
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
                var resultado = gastoBll.Editar(GastoEditado);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Gasto operativo actualizado exitosamente";
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
