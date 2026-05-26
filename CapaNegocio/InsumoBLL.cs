using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class InsumoBLL
    {
        private readonly InsumoDAL insumoDal = new InsumoDAL();

        public List<Insumo> ObtenerTodos() => insumoDal.ObtenerTodos();

        public Insumo ObtenerPorId(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID del insumo es inválido");
            return insumoDal.ObtenerPorId(id);
        }

        public bool Crear(Insumo insumo)
        {
            ValidarInsumo(insumo);
            return insumoDal.Crear(insumo);
        }

        public bool Editar(Insumo insumo)
        {
            if (insumo.Id <= 0) throw new ArgumentException("El ID del insumo es inválido");
            ValidarInsumo(insumo);
            return insumoDal.Editar(insumo);
        }

        public bool Eliminar(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID del insumo es inválido");
            return insumoDal.Eliminar(id);
        }

        private void ValidarInsumo(Insumo insumo)
        {
            if (string.IsNullOrWhiteSpace(insumo.Nombre))
                throw new ArgumentException("El nombre del insumo es obligatorio");

            if (insumo.Costo < 0)
                throw new ArgumentException("El costo no puede ser negativo");

            if (insumo.Stock < 0)
                throw new ArgumentException("El stock no puede ser negativo");

            if (insumo.StockMinimo < 0)
                throw new ArgumentException("El stock mínimo no puede ser negativo");

            if (insumo.InsumoCategoriaId <= 0)
                throw new ArgumentException("Debe seleccionar una categoría válida");

            if (insumo.ProveedorId <= 0)
                throw new ArgumentException("Debe seleccionar un proveedor válido");

            if (insumo.UnidadesMedidaId <= 0)
                throw new ArgumentException("Debe seleccionar una unidad de medida válida");

            insumo.Nombre = insumo.Nombre.Trim();
            insumo.FotoUrl = insumo.FotoUrl ?? string.Empty;
        }
    }
}
