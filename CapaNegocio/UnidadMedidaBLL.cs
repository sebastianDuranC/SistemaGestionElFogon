using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class UnidadMedidaBLL
    {
        private readonly UnidadMedidaDAL unidadmedidaDal = new UnidadMedidaDAL();

        public List<UnidadMedida> ObtenerTodos() => unidadmedidaDal.ObtenerTodos();

        public UnidadMedida ObtenerPorId(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID de la unidad de medida es inválido");
            return unidadmedidaDal.ObtenerPorId(id);
        }

        public bool Crear(UnidadMedida unidad)
        {
            if (string.IsNullOrWhiteSpace(unidad.Nombre))
                throw new ArgumentException("El nombre de la unidad de medida es obligatorio");
            if (string.IsNullOrWhiteSpace(unidad.Abreviatura))
                throw new ArgumentException("La abreviatura es obligatoria");

            unidad.Nombre = unidad.Nombre.Trim();
            unidad.Abreviatura = unidad.Abreviatura.Trim();
            return unidadmedidaDal.Crear(unidad);
        }

        public bool Editar(UnidadMedida unidad)
        {
            if (unidad.Id <= 0)
                throw new ArgumentException("El ID de la unidad de medida es inválido");

            if (string.IsNullOrWhiteSpace(unidad.Nombre))
                throw new ArgumentException("El nombre de la unidad de medida es obligatorio");
            if (string.IsNullOrWhiteSpace(unidad.Abreviatura))
                throw new ArgumentException("La abreviatura es obligatoria");

            unidad.Nombre = unidad.Nombre.Trim();
            unidad.Abreviatura = unidad.Abreviatura.Trim();
            return unidadmedidaDal.Editar(unidad);
        }

        public bool Eliminar(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID de la unidad de medida es inválido");
            return unidadmedidaDal.Eliminar(id);
        }
    }
}
