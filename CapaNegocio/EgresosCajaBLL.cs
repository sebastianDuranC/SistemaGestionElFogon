using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class EgresosCajaBLL
    {
        private readonly EgresosCajaDAL egresosDal = new EgresosCajaDAL();
        private readonly ControlCajaDAL controlCajaDal = new ControlCajaDAL();

        public List<EgresosCaja> ObtenerEgresosPorCaja(int? controlCajaId = null)
        {
            return egresosDal.ObtenerEgresosPorCaja(controlCajaId);
        }

        public bool Registrar(EgresosCaja egreso)
        {
            if (string.IsNullOrWhiteSpace(egreso.Motivo))
                throw new ArgumentException("El motivo del egreso es obligatorio");

            if (egreso.Monto <= 0)
                throw new ArgumentException("El monto del egreso debe ser mayor a cero");

            if (egreso.UsuarioId <= 0)
                throw new ArgumentException("El ID del usuario es inválido");

            // Buscar caja abierta actual
            var cajaActiva = controlCajaDal.ObtenerCajaActiva();
            if (cajaActiva == null)
                throw new ArgumentException("La caja no está abierta. Debe abrir caja antes de registrar un egreso de efectivo.");

            // Vincular el egreso a la caja activa del turno
            egreso.ControlCajaId = cajaActiva.Id;
            egreso.Motivo = egreso.Motivo.Trim();

            return egresosDal.Registrar(egreso);
        }
    }
}
