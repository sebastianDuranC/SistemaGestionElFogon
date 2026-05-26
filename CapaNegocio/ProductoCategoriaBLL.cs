using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class ProductoCategoriaBLL
    {
        private readonly ProductoCategoriaDAL productoCategoriaDal = new ProductoCategoriaDAL();

        public List<ProductoCategoria> ObtenerTodos() => productoCategoriaDal.ObtenerTodos();

        public ProductoCategoria ObtenerPorId(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID de la categoría es inválido");
            return productoCategoriaDal.ObtenerPorId(id);
        }

        public bool Crear(ProductoCategoria categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.Nombre))
                throw new ArgumentException("El nombre de la categoría es obligatorio");

            categoria.Nombre = categoria.Nombre.Trim();
            return productoCategoriaDal.Crear(categoria);
        }

        public bool Editar(ProductoCategoria categoria)
        {
            if (categoria.Id <= 0)
                throw new ArgumentException("El ID de la categoría es inválido");

            if (string.IsNullOrWhiteSpace(categoria.Nombre))
                throw new ArgumentException("El nombre de la categoría es obligatorio");

            categoria.Nombre = categoria.Nombre.Trim();
            return productoCategoriaDal.Editar(categoria);
        }

        public bool Eliminar(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID de la categoría es inválido");
            return productoCategoriaDal.Eliminar(id);
        }
    }
}
