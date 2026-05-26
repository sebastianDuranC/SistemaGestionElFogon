using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class InsumoCategoriaBLL
    {
        private readonly InsumoCategoriaDAL insumoCategoriaDal = new InsumoCategoriaDAL();

        public List<InsumoCategoria> ObtenerTodos() => insumoCategoriaDal.ObtenerTodos();

        public InsumoCategoria ObtenerPorId(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID de la categoría es inválido");
            return insumoCategoriaDal.ObtenerPorId(id);
        }

        public bool Crear(InsumoCategoria categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria.Nombre))
                throw new ArgumentException("El nombre de la categoría es obligatorio");

            categoria.Nombre = categoria.Nombre.Trim();
            return insumoCategoriaDal.Crear(categoria);
        }

        public bool Editar(InsumoCategoria categoria)
        {
            if (categoria.Id <= 0)
                throw new ArgumentException("El ID de la categoría es inválido");

            if (string.IsNullOrWhiteSpace(categoria.Nombre))
                throw new ArgumentException("El nombre de la categoría es obligatorio");

            categoria.Nombre = categoria.Nombre.Trim();
            return insumoCategoriaDal.Editar(categoria);
        }

        public bool Eliminar(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID de la categoría es inválido");
            return insumoCategoriaDal.Eliminar(id);
        }
    }
}
