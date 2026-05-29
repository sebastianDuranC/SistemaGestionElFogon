using CapaNegocio;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapaPresentacion.Pages.Compra
{
    public class CreateModel : PageModel
    {
        private readonly CompraBLL compraBLL = new();
        private readonly InsumoBLL insumoBLL = new();
        private readonly ProveedorBLL proveedorBLL = new();

        public List<Entidades.Insumo> ListaInsumos { get; set; } = new List<Entidades.Insumo>();
        public List<Entidades.Proveedor> ListaProveedores { get; set; } = new List<Entidades.Proveedor>();

        [BindProperty] public int ProveedorId { get; set; }
        [BindProperty] public List<int> InsumoSeleccionadoId { get; set; } = new List<int>();
        [BindProperty] public List<decimal> InsumoSeleccionadoCantidad { get; set; } = new List<decimal>();
        [BindProperty] public List<decimal> InsumoSeleccionadoCosto { get; set; } = new List<decimal>();
        public void OnGet()
        {
            CargarDatos();
        }

        public IActionResult OnPost()
        {
            try
            {
                var detalles = new List<DetalleCompra>();
                
                if (InsumoSeleccionadoId != null)
                {
                    for (int i = 0; i < InsumoSeleccionadoId.Count; i++)
                    {
                        var id = InsumoSeleccionadoId[i];
                        var cantidad = InsumoSeleccionadoCantidad.Count > i ? InsumoSeleccionadoCantidad[i] : 0;
                        var costo = InsumoSeleccionadoCosto.Count > i ? InsumoSeleccionadoCosto[i] : 0;

                        detalles.Add(new DetalleCompra
                        {
                            InsumoId = id,
                            Cantidad = cantidad,
                            CostoUnitario = costo
                        });
                    }
                }

                int usuarioClaimId = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                compraBLL.Crear(ProveedorId, usuarioClaimId, detalles);
                TempData["Swal_Message"] = "Compra registrada exitosamente.";
                TempData["Swal_Icon"] = "success";
                return RedirectToPage("./Index");
            }
            catch (ArgumentException ex)
            {
                TempData["Swal_Message"] = ex.Message;
                TempData["Swal_Icon"] = "error";
                CargarDatos();
                return Page();
            }
            catch (Exception ex)
            {
                TempData["Swal_Message"] = "Error inesperado: " + ex.Message;
                TempData["Swal_Icon"] = "error";
                CargarDatos();
                return Page();
            }
        }

        private void CargarDatos()
        {
            try
            {
                // Solo cargar insumos y proveedores activos
                ListaInsumos = insumoBLL.ObtenerTodos();
                ListaProveedores = proveedorBLL.ObtenerTodos();
            }
            catch (Exception ex)
            {
                TempData["Swal_Message"] = ex.Message.ToString();
                TempData["Swal_Icon"] = "error";
            }
        }
    }
}
