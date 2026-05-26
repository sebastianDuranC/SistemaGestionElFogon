using Dapper;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CapaDatos
{
    public class InsumoDAL
    {
        private readonly ConexionDAL conexion = new ConexionDAL();

        public List<Insumo> ObtenerTodos()
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Query<Insumo>(
                "sp_ListarInsumos",
                commandType: CommandType.StoredProcedure
            );
            return resultado.ToList();
        }

        public Insumo ObtenerPorId(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            return Conexion.QueryFirstOrDefault<Insumo>(
                "sp_ObtenerInsumoPorId",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public bool Crear(Insumo insumo)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_CrearInsumo",
                new
                {
                    insumo.Nombre,
                    insumo.Costo,
                    insumo.Stock,
                    insumo.StockMinimo,
                    insumo.FotoUrl,
                    insumo.InsumoCategoriaId,
                    insumo.ProveedorId,
                    insumo.UnidadesMedidaId
                },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool Editar(Insumo insumo)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EditarInsumo",
                new
                {
                    insumo.Id,
                    insumo.Nombre,
                    insumo.Costo,
                    insumo.Stock,
                    insumo.StockMinimo,
                    insumo.FotoUrl,
                    insumo.InsumoCategoriaId,
                    insumo.ProveedorId,
                    insumo.UnidadesMedidaId
                },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }

        public bool Eliminar(int id)
        {
            using var Conexion = conexion.ObtenerConexion();
            var resultado = Conexion.Execute(
                "sp_EliminarInsumo",
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
            return resultado > 0;
        }
    }
}
