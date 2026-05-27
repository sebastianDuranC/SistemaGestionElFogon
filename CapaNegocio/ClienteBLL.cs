using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class ClienteBLL
    {
        private readonly ClienteDAL clienteDal = new ClienteDAL();

        public List<Cliente> ObtenerTodos() => clienteDal.ObtenerTodos();

        public Cliente ObtenerPorId(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID del cliente es inválido");
            return clienteDal.ObtenerPorId(id);
        }

        public bool Crear(Cliente cliente)
        {
            ValidarCliente(cliente);
            return clienteDal.Crear(cliente);
        }

        public bool Editar(Cliente cliente)
        {
            if (cliente.Id <= 0) throw new ArgumentException("El ID del cliente es inválido");
            ValidarCliente(cliente);
            return clienteDal.Editar(cliente);
        }

        public bool Eliminar(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID del cliente es inválido");
            return clienteDal.Eliminar(id);
        }

        private void ValidarCliente(Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Nombre))
                throw new ArgumentException("El nombre del cliente es obligatorio");

            if (string.IsNullOrWhiteSpace(cliente.Apellido))
                throw new ArgumentException("El apellido del cliente es obligatorio");

            if (cliente.EsComerciante)
            {
                if (string.IsNullOrWhiteSpace(cliente.NumeroLocal))
                    throw new ArgumentException("El número de local es obligatorio para clientes comerciantes");

                if (string.IsNullOrWhiteSpace(cliente.Pasillo))
                    throw new ArgumentException("El pasillo es obligatorio para clientes comerciantes");
            }
            else
            {
                // Limpiar campos si no es comerciante
                cliente.NumeroLocal = string.Empty;
                cliente.Pasillo = string.Empty;
            }

            cliente.Nombre = cliente.Nombre.Trim();
            cliente.Apellido = cliente.Apellido.Trim();
            cliente.NumeroLocal = cliente.NumeroLocal?.Trim() ?? string.Empty;
            cliente.Pasillo = cliente.Pasillo?.Trim() ?? string.Empty;
        }
    }
}
