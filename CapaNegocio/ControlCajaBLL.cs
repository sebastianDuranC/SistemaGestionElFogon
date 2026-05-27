using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class ControlCajaBLL
    {
        private readonly ControlCajaDAL controlCajaDal = new ControlCajaDAL();

        public ControlCaja ObtenerCajaActiva() => controlCajaDal.ObtenerCajaActiva();

        public List<ControlCaja> ObtenerHistorico() => controlCajaDal.ObtenerHistorico();

        public bool AbrirCaja(ControlCaja cc)
        {
            if (cc.MontoApertura < 0)
                throw new ArgumentException("El monto de apertura no puede ser negativo");

            if (cc.UsuarioId <= 0)
                throw new ArgumentException("El ID del usuario es inválido");

            // Verificar si ya existe una caja abierta
            var cajaActiva = controlCajaDal.ObtenerCajaActiva();
            if (cajaActiva != null)
                throw new ArgumentException("No se puede abrir la caja porque ya hay un turno activo abierto");

            // Forzar NegocioId a 1
            cc.NegocioId = 1;

            return controlCajaDal.AbrirCaja(cc);
        }

        public bool CerrarCaja(decimal montoCierreReal)
        {
            if (montoCierreReal < 0)
                throw new ArgumentException("El monto de cierre real no puede ser negativo");

            // Obtener caja abierta
            var cajaActiva = controlCajaDal.ObtenerCajaActiva();
            if (cajaActiva == null)
                throw new ArgumentException("No hay ninguna caja abierta en este momento para poder realizar el cierre");

            // Obtener el resumen financiero calculado por la base de datos
            var resumen = controlCajaDal.ObtenerResumenCaja(cajaActiva.Id);
            if (resumen == null)
                throw new Exception("Error al calcular el resumen de la caja activa");

            cajaActiva.MontoCierreEsperado = resumen.MontoCierreEsperado;
            cajaActiva.MontoCierreReal = montoCierreReal;
            cajaActiva.Diferencial = montoCierreReal - resumen.MontoCierreEsperado;

            return controlCajaDal.CerrarCaja(cajaActiva);
        }

        public ControlCajaResumen ObtenerResumenCaja(int controlCajaId)
        {
            if (controlCajaId <= 0)
                throw new ArgumentException("El ID de control de caja es inválido");

            return controlCajaDal.ObtenerResumenCaja(controlCajaId);
        }
    }
}
