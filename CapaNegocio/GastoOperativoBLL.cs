using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class GastoOperativoBLL
    {
        private readonly GastoOperativoDAL gastoDal = new GastoOperativoDAL();

        public List<GastoOperativo> ObtenerTodos()
        {
            return gastoDal.ObtenerTodos();
        }

        public GastoOperativo ObtenerPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del gasto operativo debe ser mayor que cero.");

            return gastoDal.ObtenerPorId(id);
        }

        public bool Crear(GastoOperativo gasto)
        {
            if (gasto.UsuarioId <= 0)
                throw new ArgumentException("El ID del usuario es inválido.");

            ValidarGasto(gasto);
            return gastoDal.Crear(gasto);
        }

        public bool Editar(GastoOperativo gasto)
        {
            if (gasto.Id <= 0)
                throw new ArgumentException("El ID del gasto operativo debe ser mayor que cero.");

            ValidarGasto(gasto);
            return gastoDal.Editar(gasto);
        }

        public bool Eliminar(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del gasto operativo debe ser mayor que cero.");

            return gastoDal.Eliminar(id);
        }

        private void ValidarGasto(GastoOperativo gasto)
        {
            if (string.IsNullOrWhiteSpace(gasto.Concepto))
                throw new ArgumentException("El concepto del gasto es obligatorio.");

            if (gasto.Monto <= 0)
                throw new ArgumentException("El monto del gasto debe ser mayor que cero.");

            gasto.Concepto = gasto.Concepto.Trim();
        }
    }
}
