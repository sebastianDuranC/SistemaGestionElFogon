using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class MetodoPagoBLL
    {
        private readonly MetodoPagoDAL metodoPagoDal = new MetodoPagoDAL();

        public List<MetodoPago> ObtenerTodos() => metodoPagoDal.ObtenerTodos();

        public MetodoPago ObtenerPorId(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID del método de pago es inválido");
            return metodoPagoDal.ObtenerPorId(id);
        }

        public bool Crear(MetodoPago metodoPago)
        {
            ValidarMetodoPago(metodoPago);
            return metodoPagoDal.Crear(metodoPago);
        }

        public bool Editar(MetodoPago metodoPago)
        {
            if (metodoPago.Id <= 0) throw new ArgumentException("El ID del método de pago es inválido");
            ValidarMetodoPago(metodoPago);
            return metodoPagoDal.Editar(metodoPago);
        }

        public bool Eliminar(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID del método de pago es inválido");
            return metodoPagoDal.Eliminar(id);
        }

        private void ValidarMetodoPago(MetodoPago metodoPago)
        {
            if (string.IsNullOrWhiteSpace(metodoPago.Nombre))
                throw new ArgumentException("El nombre del método de pago es obligatorio");

            metodoPago.Nombre = metodoPago.Nombre.Trim();
        }
    }
}
