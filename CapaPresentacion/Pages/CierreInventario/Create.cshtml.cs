using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CapaNegocio;
using Entidades;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System;

namespace CapaPresentacion.Pages.CierreInventario
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly CierreInventarioBLL cierreBll = new CierreInventarioBLL();
        private readonly InsumoBLL insumoBll = new InsumoBLL();

        public List<Entidades.Insumo> ListaInsumos { get; set; } = new List<Entidades.Insumo>();

        [BindProperty]
        public string CierreJson { get; set; } = string.Empty;

        public void OnGet()
        {
            ListaInsumos = insumoBll.ObtenerTodos();
        }

        public class CierreItemInput
        {
            public int InsumoId { get; set; }
            public decimal CantidadTeorica { get; set; }
            public decimal CantidadReal { get; set; }
            public decimal Diferencia { get; set; }
            public string Observacion { get; set; }
        }

        public IActionResult OnPost()
        {
            try
            {
                var claimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(claimId))
                    throw new Exception("Sesión inválida o expirada");

                int usuarioId = int.Parse(claimId);

                if (string.IsNullOrWhiteSpace(CierreJson))
                    throw new ArgumentException("No hay datos de conteo para guardar");

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var itemsInput = JsonSerializer.Deserialize<List<CierreItemInput>>(CierreJson, options);

                if (itemsInput == null || itemsInput.Count == 0)
                    throw new ArgumentException("Los datos de conteo de inventario están vacíos");

                var listaCierres = new List<Entidades.CierreInventario>();
                foreach (var item in itemsInput)
                {
                    listaCierres.Add(new Entidades.CierreInventario
                    {
                        InsumoId = item.InsumoId,
                        CantidadTeorica = item.CantidadTeorica,
                        CantidadReal = item.CantidadReal,
                        Diferencia = item.Diferencia,
                        Observacion = item.Observacion,
                        Estado = true
                    });
                }

                var resultado = cierreBll.RegistrarCierre(listaCierres, usuarioId);
                if (resultado)
                {
                    TempData["Swal_Message"] = "Cierre de inventario registrado exitosamente";
                    TempData["Swal_Icon"] = "success";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData["Swal_Message"] = "No se pudo registrar el cierre de inventario";
                    TempData["Swal_Icon"] = "error";
                }
            }
            catch (Exception ex)
            {
                TempData["Swal_Message"] = ex.Message;
                TempData["Swal_Icon"] = "error";
            }

            // Recargar datos en caso de error
            ListaInsumos = insumoBll.ObtenerTodos();
            return Page();
        }
    }
}
