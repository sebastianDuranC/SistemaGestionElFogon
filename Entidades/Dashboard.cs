using System.Collections.Generic;

namespace Entidades
{
    public class DashboardResumen
    {
        //metricas principales expuestas en tarjetas
        public decimal TotalVentasHoy { get; set; }
        public decimal VentasMesBs { get; set; }
        public int TotalInsumos { get; set; }
        public int InsumosStockBajoCount { get; set; }

        //listas de apoyo para los graficos y la tabla
        public List<ProductoMasVendido> ProductosMasVendidos { get; set; } = new();
        public List<MetodoPagoMasUsado> MetodosPagoMasUsados { get; set; } = new();
    }

    public class ProductoMasVendido
    {
        public string ProductoNombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }

    public class MetodoPagoMasUsado
    {
        public string MetodoPagoNombre { get; set; } = string.Empty;
        public decimal TotalMonto { get; set; }
    }
}
