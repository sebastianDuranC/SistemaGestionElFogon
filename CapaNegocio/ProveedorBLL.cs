using CapaDatos;
using Entidades;
using System;
using System.Collections.Generic;

namespace CapaNegocio
{
    public class ProveedorBLL
    {
        private readonly ProveedorDAL proveedorDAL = new ProveedorDAL();

        public List<Proveedor> ObtenerTodos() => proveedorDAL.ObtenerTodos();

        public Proveedor ObtenerPorId(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID del proveedor es inválido");
            return proveedorDAL.ObtenerPorId(id);
        }

        public bool CrearProveedor(Proveedor proveedor)
        {
            if (string.IsNullOrWhiteSpace(proveedor.Nombre))
                throw new ArgumentException("El nombre del proveedor es obligatorio");

            proveedor.Nombre = proveedor.Nombre.Trim();
            proveedor.Apellido = (proveedor.Apellido ?? "").Trim();
            proveedor.Contacto = (proveedor.Contacto ?? "").Trim();

            return proveedorDAL.CrearProveedor(proveedor);
        }

        public bool EditarProveedor(Proveedor proveedor)
        {
            if (proveedor.Id <= 0)
                throw new ArgumentException("El ID del proveedor es inválido");

            if (string.IsNullOrWhiteSpace(proveedor.Nombre))
                throw new ArgumentException("El nombre del proveedor es obligatorio");

            proveedor.Nombre = proveedor.Nombre.Trim();
            proveedor.Apellido = (proveedor.Apellido ?? "").Trim();
            proveedor.Contacto = (proveedor.Contacto ?? "").Trim();

            return proveedorDAL.EditarProveedor(proveedor);
        }

        public bool EliminarProveedor(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID del proveedor es inválido");
            return proveedorDAL.EliminarProveedor(id);
        }
    }
}
