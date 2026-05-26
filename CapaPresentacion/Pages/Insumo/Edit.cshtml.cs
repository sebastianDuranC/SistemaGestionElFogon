using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Collections.Generic;
using System.IO;
using System;
using System.Threading.Tasks;

namespace CapaPresentacion.Pages.Insumo
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly InsumoBLL insumoBll = new InsumoBLL();
        private readonly InsumoCategoriaBLL insumoCategoriaBll = new InsumoCategoriaBLL();
        private readonly UnidadMedidaBLL unidadMedidaBll = new UnidadMedidaBLL();
        private readonly ProveedorBLL proveedorBll = new ProveedorBLL();
        private readonly IWebHostEnvironment _environment;

        public EditModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [BindProperty]
        public Entidades.Insumo InsumoEditado { get; set; } = new Entidades.Insumo();

        [BindProperty]
        public IFormFile? Foto { get; set; }

        public List<Entidades.InsumoCategoria> ListaCategorias { get; set; } = new();
        public List<Entidades.UnidadMedida> ListaUnidades { get; set; } = new();
        public List<Entidades.Proveedor> ListaProveedores { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            InsumoEditado = insumoBll.ObtenerPorId(id);
            if (InsumoEditado == null)
            {
                TempData["Swal_Message"] = "El insumo no existe";
                TempData["Swal_Icon"] = "error";
                return RedirectToPage("Index");
            }

            CargarListas();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (Foto != null)
                {
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "Fotos");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Foto.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Foto.CopyToAsync(fileStream);
                    }

                    // Guardar nueva ruta
                    InsumoEditado.FotoUrl = "/Fotos/" + uniqueFileName;
                }
                else
                {
                    // Si no se subió una foto nueva, mantener la actual
                    var insumoActual = insumoBll.ObtenerPorId(InsumoEditado.Id);
                    if (insumoActual != null)
                    {
                        InsumoEditado.FotoUrl = insumoActual.FotoUrl;
                    }
                }

                var resultado = insumoBll.Editar(InsumoEditado);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Insumo actualizado correctamente";
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

            CargarListas();
            return Page();
        }

        private void CargarListas()
        {
            ListaCategorias = insumoCategoriaBll.ObtenerTodos();
            ListaUnidades = unidadMedidaBll.ObtenerTodos();
            ListaProveedores = proveedorBll.ObtenerTodos();
        }
    }
}
