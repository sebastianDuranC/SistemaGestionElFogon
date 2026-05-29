using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CompraBLL
    {
        private readonly CompraDAL compraDAL = new CompraDAL();

        public List<Compra> Listar()
        {
            return compraDAL.Listar();
        }

        public Compra ObtenerPorId(int id)
        {
            return compraDAL.ObtenerPorId(id);
        }

        public List<DetalleCompra> ObtenerDetalles(int compraId)
        {
            return compraDAL.ObtenerDetalles(compraId);
        }

        public int Crear(int proveedorId, int usuarioId, List<DetalleCompra> detalles)
        {
            if (proveedorId <= 0)
                throw new ArgumentException("Debe seleccionar un proveedor válido.");
                
            if (detalles == null || detalles.Count == 0)
                throw new ArgumentException("Debe agregar al menos un insumo a la compra.");

            decimal totalCalculado = 0;
            foreach (var det in detalles)
            {
                if (det.InsumoId <= 0)
                    throw new ArgumentException("El ID del insumo debe ser mayor que cero.");
                if (det.Cantidad <= 0)
                    throw new ArgumentException("La cantidad de cada insumo debe ser mayor a 0.");
                if (det.CostoUnitario < 0)
                    throw new ArgumentException("El costo unitario de cada insumo no puede ser negativo.");
                    
                det.Subtotal = det.Cantidad * det.CostoUnitario;
                totalCalculado += det.Subtotal;
            }

            return compraDAL.Crear(proveedorId, usuarioId, totalCalculado, detalles);
        }

        public bool Anular(int id, int usuarioId)
        {
            if (id <= 0)
                throw new ArgumentException("ID de compra inválido.");
            
            return compraDAL.Anular(id, usuarioId);
        }
    }
}
