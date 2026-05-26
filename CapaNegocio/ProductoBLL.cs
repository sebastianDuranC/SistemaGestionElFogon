using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class ProductoBLL
    {
        private readonly ProductoDAL productoDal = new ProductoDAL();
        private readonly ProductoInsumoBLL _insumoBll = new ProductoInsumoBLL();

        public List<Producto> ObtenerTodos() => productoDal.ObtenerTodos();

        public Producto ObtenerPorId(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID del producto es inválido");
            return productoDal.ObtenerPorId(id);
        }

        public bool Crear(Producto producto, List<ProductoInsumo> insumos)
        {
            ValidarProducto(producto);
            _insumoBll.ValidarReceta(insumos);
            return productoDal.CrearProducto(producto, insumos);
        }

        public bool Editar(Producto producto, List<ProductoInsumo> insumos)
        {
            if (producto.Id <= 0) throw new ArgumentException("El ID del producto es inválido");
            ValidarProducto(producto);
            _insumoBll.ValidarReceta(insumos);
            return productoDal.EditarProducto(producto, insumos);
        }

        public bool Eliminar(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID del producto es inválido");
            return productoDal.EliminarProducto(id);
        }

        private void ValidarProducto(Producto producto)
        {
            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new ArgumentException("El nombre del producto es obligatorio");

            if (producto.Precio < 0)
                throw new ArgumentException("El precio no puede ser negativo");

            if (producto.ProductoCategoriaId <= 0)
                throw new ArgumentException("Debe seleccionar una categoría válida");

            producto.Nombre = producto.Nombre.Trim();
            producto.FotoUrl = producto.FotoUrl ?? string.Empty;
        }
    }
}
