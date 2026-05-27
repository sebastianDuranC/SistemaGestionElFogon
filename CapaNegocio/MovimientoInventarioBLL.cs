using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class MovimientoInventarioBLL
    {
        private readonly MovimientoInventarioDAL movimientoDal = new MovimientoInventarioDAL();
        private readonly InsumoDAL insumoDal = new InsumoDAL();

        public List<MovimientoInventario> ObtenerTodos() => movimientoDal.ObtenerTodos();

        public bool Registrar(MovimientoInventario movimiento)
        {
            if (movimiento.InsumoId <= 0)
                throw new ArgumentException("Debe seleccionar un insumo válido");

            if (movimiento.UsuarioId <= 0)
                throw new ArgumentException("El ID del usuario es inválido");

            if (string.IsNullOrWhiteSpace(movimiento.TipoMovimiento))
                throw new ArgumentException("El tipo de movimiento es obligatorio");

            var insumo = insumoDal.ObtenerPorId(movimiento.InsumoId);
            if (insumo == null)
                throw new ArgumentException("El insumo seleccionado no existe");

            // Si es merma, el usuario ingresa cantidad positiva, la validamos y convertimos a negativo para descontar
            if (movimiento.TipoMovimiento == "Merma")
            {
                if (movimiento.Cantidad <= 0)
                    throw new ArgumentException("La cantidad de merma debe ser mayor a cero");

                if (insumo.Stock < movimiento.Cantidad)
                    throw new ArgumentException($"No hay suficiente stock. Stock actual: {insumo.Stock}, cantidad de merma: {movimiento.Cantidad}");

                // Guardar como cantidad negativa para que el UPDATE Insumo sume un valor negativo (descuente)
                movimiento.Cantidad = -movimiento.Cantidad;
            }
            else
            {
                if (movimiento.Cantidad == 0)
                    throw new ArgumentException("La cantidad del movimiento no puede ser cero");
            }

            movimiento.Observacion = movimiento.Observacion?.Trim() ?? string.Empty;
            return movimientoDal.Registrar(movimiento);
        }
    }
}
