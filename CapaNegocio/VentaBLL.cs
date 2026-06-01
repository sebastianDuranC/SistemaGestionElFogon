using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class VentaBLL
    {
        private readonly VentaDAL ventaDAL = new VentaDAL();
        private readonly ClienteBLL clienteBLL = new ClienteBLL();

        public List<Venta> Listar()
        {
            return ventaDAL.Listar();
        }

        public Venta ObtenerPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de la venta debe ser mayor que cero.");
            return ventaDAL.ObtenerPorId(id);
        }

        public List<DetalleVenta> ObtenerDetallesVenta(int ventaId)
        {
            if (ventaId <= 0)
                throw new ArgumentException("El ID de la venta debe ser mayor que cero.");
            return ventaDAL.ObtenerDetallesVenta(ventaId);
        }

        public List<DetallePago> ObtenerDetallesPago(int ventaId)
        {
            if (ventaId <= 0)
                throw new ArgumentException("El ID de la venta debe ser mayor que cero.");
            return ventaDAL.ObtenerDetallesPago(ventaId);
        }

        public int Crear(int? clienteId, int usuarioId, bool enLocal, bool? platoPrestado, List<DetalleVenta> detallesVenta, List<DetallePago> detallesPago)
        {
            if (usuarioId <= 0)
                throw new ArgumentException("El ID de usuario es inválido.");

            if (detallesVenta == null || detallesVenta.Count == 0)
                throw new ArgumentException("Debe agregar al menos un producto a la venta.");

            if (detallesPago == null || detallesPago.Count == 0)
                throw new ArgumentException("Debe ingresar al menos un método de pago.");

            // Validar cliente comerciante y platos prestados
            bool esComerciante = false;
            if (clienteId.HasValue && clienteId.Value > 0)
            {
                var cliente = clienteBLL.ObtenerPorId(clienteId.Value);
                if (cliente != null)
                {
                    esComerciante = cliente.EsComerciante;
                }
            }

            if (platoPrestado == true)
            {
                if (enLocal)
                    throw new ArgumentException("No se pueden prestar platos si el consumo es en el local.");
                if (!esComerciante)
                    throw new ArgumentException("Solo se pueden prestar platos a clientes de tipo comerciante.");
            }

            // Calcular total de la venta
            decimal totalCalculado = 0;
            foreach (var det in detallesVenta)
            {
                if (det.ProductoId <= 0)
                    throw new ArgumentException("ID de producto inválido.");
                if (det.Cantidad <= 0)
                    throw new ArgumentException("La cantidad de cada producto debe ser mayor a cero.");
                if (det.PrecioUnitario < 0)
                    throw new ArgumentException("El precio unitario no puede ser negativo.");

                det.SubTotal = det.Cantidad * det.PrecioUnitario;
                totalCalculado += det.SubTotal;
            }

            // Calcular total pagado
            decimal totalPagado = 0;
            foreach (var pago in detallesPago)
            {
                if (pago.MetodoPagoId <= 0)
                    throw new ArgumentException("ID de método de pago inválido.");
                if (pago.Monto <= 0)
                    throw new ArgumentException("El monto de pago debe ser mayor a cero.");

                totalPagado += pago.Monto;
            }

            if (totalPagado < totalCalculado)
                throw new ArgumentException("El monto pagado es insuficiente para cubrir el total de la venta.");

            decimal cambio = totalPagado - totalCalculado;

            return ventaDAL.Crear(
                clienteId, 
                usuarioId, 
                totalCalculado, 
                enLocal, 
                platoPrestado, 
                totalPagado, 
                cambio, 
                detallesVenta, 
                detallesPago
            );
        }

        public bool Anular(int id, int usuarioId)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de la venta es inválido.");
            if (usuarioId <= 0)
                throw new ArgumentException("El ID de usuario es inválido.");

            return ventaDAL.Anular(id, usuarioId);
        }

        public bool DevolverPlatos(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID de la venta es inválido.");

            return ventaDAL.DevolverPlatos(id);
        }
    }
}
