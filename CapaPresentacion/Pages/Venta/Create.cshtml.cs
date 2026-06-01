using CapaNegocio;
using Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CapaPresentacion.Pages.Venta
{
    public class CreateModel : PageModel
    {
        private readonly VentaBLL ventaBLL = new VentaBLL();
        private readonly ProductoBLL productoBLL = new ProductoBLL();
        private readonly ClienteBLL clienteBLL = new ClienteBLL();
        private readonly MetodoPagoBLL metodoPagoBLL = new MetodoPagoBLL();

        public List<Entidades.Producto> ListaProductos { get; set; } = new List<Entidades.Producto>();
        public List<Entidades.Cliente> ListaClientes { get; set; } = new List<Entidades.Cliente>();
        public List<Entidades.MetodoPago> ListaMetodosPago { get; set; } = new List<Entidades.MetodoPago>();

        [BindProperty] public int? ClienteId { get; set; }
        [BindProperty] public bool EnLocal { get; set; }
        [BindProperty] public bool PlatoPrestado { get; set; }

        [BindProperty] public List<int> ProductoSeleccionadoId { get; set; } = new List<int>();
        [BindProperty] public List<int> ProductoSeleccionadoCantidad { get; set; } = new List<int>();
        [BindProperty] public List<decimal> ProductoSeleccionadoPrecio { get; set; } = new List<decimal>();

        [BindProperty] public List<int> PagoMetodoId { get; set; } = new List<int>();
        [BindProperty] public List<decimal> PagoMonto { get; set; } = new List<decimal>();

        public void OnGet()
        {
            CargarDatos();
        }

        public IActionResult OnPost()
        {
            try
            {
                var detallesVenta = new List<DetalleVenta>();
                if (ProductoSeleccionadoId != null)
                {
                    for (int i = 0; i < ProductoSeleccionadoId.Count; i++)
                    {
                        var prodId = ProductoSeleccionadoId[i];
                        var cantidad = ProductoSeleccionadoCantidad.Count > i ? ProductoSeleccionadoCantidad[i] : 0;
                        var precio = ProductoSeleccionadoPrecio.Count > i ? ProductoSeleccionadoPrecio[i] : 0;

                        detallesVenta.Add(new DetalleVenta
                        {
                            ProductoId = prodId,
                            Cantidad = cantidad,
                            PrecioUnitario = precio
                        });
                    }
                }

                var detallesPago = new List<DetallePago>();
                if (PagoMetodoId != null)
                {
                    for (int i = 0; i < PagoMetodoId.Count; i++)
                    {
                        var metodoId = PagoMetodoId[i];
                        var monto = PagoMonto.Count > i ? PagoMonto[i] : 0;

                        if (monto > 0)
                        {
                            detallesPago.Add(new DetallePago
                            {
                                MetodoPagoId = metodoId,
                                Monto = monto
                            });
                        }
                    }
                }

                int usuarioClaimId = Convert.ToInt32(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

                // Solo pasar el ID del cliente si es mayor a 0 (por si selecciona la opción vacía o por defecto)
                int? clienteIdNull = (ClienteId.HasValue && ClienteId.Value > 0) ? ClienteId : null;

                // El préstamo de platos solo es válido si no es consumo en local
                bool? platoPrestadoValor = !EnLocal ? (bool?)PlatoPrestado : null;

                ventaBLL.Crear(clienteIdNull, usuarioClaimId, EnLocal, platoPrestadoValor, detallesVenta, detallesPago);

                TempData["Swal_Message"] = "Venta registrada exitosamente.";
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
                ListaProductos = productoBLL.ObtenerTodos();
                ListaClientes = clienteBLL.ObtenerTodos();
                ListaMetodosPago = metodoPagoBLL.ObtenerTodos();
            }
            catch (Exception ex)
            {
                TempData["Swal_Message"] = "Error al cargar datos: " + ex.Message;
                TempData["Swal_Icon"] = "error";
            }
        }
    }
}
