using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System;

namespace CapaPresentacion.Pages.UnidadesMedida
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly UnidadMedidaBLL unidadmedidaBll = new UnidadMedidaBLL();

        [BindProperty]
        public Entidades.UnidadMedida UnidadEditada { get; set; } = new Entidades.UnidadMedida();

        public IActionResult OnGet(int id)
        {
            UnidadEditada = unidadmedidaBll.ObtenerPorId(id);
            if (UnidadEditada == null)
            {
                TempData["Swal_Message"] = "La unidad de medida no existe";
                TempData["Swal_Icon"] = "error";
                return RedirectToPage("Index");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                var resultado = unidadmedidaBll.Editar(UnidadEditada);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Unidad modificada correctamente";
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
