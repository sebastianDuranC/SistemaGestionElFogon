using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapaNegocio
{
    public class NegocioBLL
    {
        private readonly NegocioDAL negocio = new NegocioDAL();
        public Negocio obtenerDatosNegocio() => negocio.ObtenerDatosNegocio();

        public Negocio obtenerDatosNegocioId (int id)
        {
            if (id < 0) return null;
            return negocio.ObtenerDatosNegocioPorId(id);
        }

        public bool editarDatosNegocio(Negocio negocio)
        {
            if (negocio == null || negocio.Id <= 0 || string.IsNullOrEmpty(negocio.Nombre) || string.IsNullOrEmpty(negocio.Direccion))
            {
                return false;
            }
            return this.negocio.EditarDatosNegocio(negocio);
        }
    }
}
