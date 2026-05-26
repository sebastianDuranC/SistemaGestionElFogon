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
    public class CreateModel : PageModel
    {
        private readonly InsumoBLL insumoBll = new InsumoBLL();
        private readonly InsumoCategoriaBLL insumoCategoriaBll = new InsumoCategoriaBLL();
        private readonly UnidadMedidaBLL unidadMedidaBll = new UnidadMedidaBLL();
        private readonly ProveedorBLL proveedorBll = new ProveedorBLL();
        private readonly IWebHostEnvironment _environment;

        public CreateModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [BindProperty]
        public Entidades.Insumo NuevoInsumo { get; set; } = new Entidades.Insumo();

        [BindProperty]
        public IFormFile? Foto { get; set; }

        public List<Entidades.InsumoCategoria> ListaCategorias { get; set; } = new();
        public List<Entidades.UnidadMedida> ListaUnidades { get; set; } = new();
        public List<Entidades.Proveedor> ListaProveedores { get; set; } = new();

        public void OnGet()
        {
            CargarListas();
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

                    NuevoInsumo.FotoUrl = "/Fotos/" + uniqueFileName;
                }
                else
                {
                    NuevoInsumo.FotoUrl = string.Empty;
                }

                NuevoInsumo.Estado = true;
                var resultado = insumoBll.Crear(NuevoInsumo);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Insumo registrado correctamente";
                    TempData["Swal_Icon"] = "success";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo registrar el insumo";
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
