using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System;
using System.IO;
using System.Threading.Tasks;

namespace CapaPresentacion.Pages.Negocio
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly NegocioBLL negocioBLL = new NegocioBLL();
        private readonly IWebHostEnvironment _environment;

        public EditModel(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [BindProperty]
        public Entidades.Negocio DatoNegocio { get; set; } = new Entidades.Negocio();

        [BindProperty]
        public IFormFile? Foto { get; set; }

        public void OnGet()
        {
            // Fijo ID = 1 para el negocio
            DatoNegocio = negocioBLL.obtenerDatosNegocioId(1) ?? new Entidades.Negocio();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (Foto != null)
                {
                    // Carpeta física: wwwroot/Fotos
                    string uploadsFolder = Path.Combine(_environment.WebRootPath, "Fotos");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Generar un nombre único para la foto
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(Foto.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Guardar la foto en disco
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Foto.CopyToAsync(fileStream);
                    }

                    // Guardar la ruta relativa en la base de datos
                    DatoNegocio.LogoUrl = "/Fotos/" + uniqueFileName;
                }
                else
                {
                    // Si no se subió una nueva foto, conservar la foto actual
                    var negocioActual = negocioBLL.obtenerDatosNegocioId(1);
                    if (negocioActual != null)
                    {
                        DatoNegocio.LogoUrl = negocioActual.LogoUrl;
                    }
                }

                // ID fijo = 1 y Estado = true
                DatoNegocio.Id = 1;
                DatoNegocio.Estado = true;

                var resultado = negocioBLL.editarDatosNegocio(DatoNegocio);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Datos del negocio editado correctamente";
                    TempData["Swal_Icon"] = "success";
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudieron guardar los cambios.";
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
