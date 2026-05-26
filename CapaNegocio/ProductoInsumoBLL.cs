using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class ProductoInsumoBLL
    {
        private readonly ProductoInsumoDAL _dal = new ProductoInsumoDAL();

        public List<ProductoInsumo> ObtenerInsumosPorProducto(int productoId)
        {
            if (productoId <= 0)
                throw new ArgumentException("El ID del producto es inválido.");

            return _dal.ObtenerInsumosPorProducto(productoId);
        }

        public void ValidarReceta(List<ProductoInsumo> insumos)
        {
            if (insumos != null)
            {
                foreach (var insumo in insumos)
                {
                    if (insumo.InsumoId <= 0)
                        throw new ArgumentException("ID de insumo inválido en la receta");
                    
                    if (insumo.Cantidad <= 0)
                        throw new ArgumentException("La cantidad para cada insumo debe ser mayor a 0");
                    
                    if (insumo.Tipo != "Comestible" && insumo.Tipo != "Descartable")
                        throw new ArgumentException("El tipo de insumo debe ser 'Comestible' o 'Descartable'");
                }
            }
        }
    }
}
