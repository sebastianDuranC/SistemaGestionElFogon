using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class CierreInventarioBLL
    {
        private readonly CierreInventarioDAL cierreDal = new CierreInventarioDAL();
        private readonly InsumoDAL insumoDal = new InsumoDAL();

        public List<CierreInventario> ObtenerTodos() => cierreDal.ObtenerTodos();

        public bool RegistrarCierre(List<CierreInventario> cierres, int usuarioId)
        {
            if (usuarioId <= 0)
                throw new ArgumentException("El ID del usuario es inválido");

            if (cierres == null || cierres.Count == 0)
                throw new ArgumentException("No se han enviado insumos para realizar el cierre de inventario");

            foreach (var item in cierres)
            {
                if (item.InsumoId <= 0)
                    throw new ArgumentException("ID de insumo inválido en la lista de cierre");

                if (item.CantidadReal < 0)
                    throw new ArgumentException("La cantidad real física no puede ser negativa");

                var insumo = insumoDal.ObtenerPorId(item.InsumoId);
                if (insumo == null)
                    throw new ArgumentException($"El insumo con ID {item.InsumoId} no existe");

                // Asignar cantidad teórica actual en base de datos para seguridad
                item.CantidadTeorica = insumo.Stock;
                // Calcular diferencia exacta: Real - Teórica
                item.Diferencia = item.CantidadReal - item.CantidadTeorica;
                item.Observacion = item.Observacion?.Trim() ?? string.Empty;
            }

            return cierreDal.RegistrarCierre(cierres, usuarioId);
        }
    }
}
