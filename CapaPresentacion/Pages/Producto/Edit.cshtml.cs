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
    public class EditModel : PageModel
    {
        private readonly ProductoBLL productoBll = new ProductoBLL();
        private readonly ProductoInsumoBLL productoInsumoBll = new ProductoInsumoBLL();
        private readonly ProductoCategoriaBLL productoCategoriaBll = new ProductoCategoriaBLL();
        private readonly InsumoBLL insumoBll = new InsumoBLL();
        private readonly IWebHostEnvironment _environment;

        public EditModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [BindProperty]
        public Entidades.Producto ProductoEditado { get; set; } = new Entidades.Producto();

        [BindProperty]
        public IFormFile? Foto { get; set; }

        [BindProperty]
        public string InsumosJson { get; set; } = "[]";

        public List<Entidades.ProductoCategoria> ListaCategorias { get; set; } = new();
        public List<Entidades.Insumo> ListaInsumos { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            ProductoEditado = productoBll.ObtenerPorId(id);
            if (ProductoEditado == null)
            {
                TempData["Swal_Message"] = "El producto no existe";
                TempData["Swal_Icon"] = "error";
                return RedirectToPage("Index");
            }

            var insumosReceta = productoInsumoBll.ObtenerInsumosPorProducto(id);
            InsumosJson = JsonSerializer.Serialize(insumosReceta);

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

                    ProductoEditado.FotoUrl = "/Fotos/" + uniqueFileName;
                }
                else
                {
                    // Mantener la foto existente
                    var productoActual = productoBll.ObtenerPorId(ProductoEditado.Id);
                    if (productoActual != null)
                    {
                        ProductoEditado.FotoUrl = productoActual.FotoUrl;
                    }
                }

                // Deserializar la receta de insumos
                List<ProductoInsumo> insumos = new List<ProductoInsumo>();
                if (!string.IsNullOrWhiteSpace(InsumosJson))
                {
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    insumos = JsonSerializer.Deserialize<List<ProductoInsumo>>(InsumosJson, options) ?? new List<ProductoInsumo>();
                }

                var resultado = productoBll.Editar(ProductoEditado, insumos);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Producto y receta actualizados correctamente";
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
            ListaCategorias = productoCategoriaBll.ObtenerTodos();
            ListaInsumos = insumoBll.ObtenerTodos();
        }
    }
}
