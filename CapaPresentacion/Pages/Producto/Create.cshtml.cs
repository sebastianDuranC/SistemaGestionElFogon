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
using System.Text.Json;
using System.Threading.Tasks;

namespace CapaPresentacion.Pages.Producto
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ProductoBLL _productoBll = new ProductoBLL();
        private readonly ProductoCategoriaBLL _categoriaBll = new ProductoCategoriaBLL();
        private readonly InsumoBLL _insumoBll = new InsumoBLL();
        private readonly IWebHostEnvironment _environment;

        public CreateModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [BindProperty]
        public Entidades.Producto NuevoProducto { get; set; } = new Entidades.Producto();

        [BindProperty]
        public IFormFile? Foto { get; set; }

        [BindProperty]
        public string InsumosJson { get; set; } = "[]";

        public List<Entidades.ProductoCategoria> ListaCategorias { get; set; } = new();
        public List<Entidades.Insumo> ListaInsumos { get; set; } = new();

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

                    NuevoProducto.FotoUrl = "/Fotos/" + uniqueFileName;
                }
                else
                {
                    NuevoProducto.FotoUrl = string.Empty;
                }

                // Deserializar la receta de insumos
                List<ProductoInsumo> insumos = new List<ProductoInsumo>();
                if (!string.IsNullOrWhiteSpace(InsumosJson))
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    insumos = JsonSerializer.Deserialize<List<ProductoInsumo>>(InsumosJson, options) ?? new List<ProductoInsumo>();
                }

                NuevoProducto.Estado = true;
                var resultado = _productoBll.Crear(NuevoProducto, insumos);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Producto y receta creados correctamente";
                    TempData["Swal_Icon"] = "success";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo registrar el producto";
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
            ListaCategorias = _categoriaBll.ObtenerTodos();
            ListaInsumos = _insumoBll.ObtenerTodos();
        }
    }
}
