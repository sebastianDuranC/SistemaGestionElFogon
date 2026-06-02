USE Prueba;
GO
CREATE OR ALTER PROCEDURE sp_ObtenerDatosDashboard
AS
BEGIN
    SET NOCOUNT ON;

    --variables para almacenar valores de tarjetas
    DECLARE @totalVentasHoy DECIMAL(18,2) = 0;
    DECLARE @ventasMesBs DECIMAL(18,2) = 0;
    DECLARE @totalInsumos INT = 0;
    DECLARE @insumosStockBajoCount INT = 0;

    --tarjeta de total ventas hoy
    SELECT @totalVentasHoy = ISNULL(SUM(Total), 0)
    FROM Venta
    WHERE Estado = 1 AND CAST(Fecha AS DATE) = CAST(GETDATE() AS DATE);

    --tarjeta de ventas del mes actual
    SELECT @ventasMesBs = ISNULL(SUM(Total), 0)
    FROM Venta
    WHERE Estado = 1 
      AND YEAR(Fecha) = YEAR(GETDATE()) 
      AND MONTH(Fecha) = MONTH(GETDATE());

    --tarjeta de total de insumos
    SELECT @totalInsumos = COUNT(*)
    FROM Insumo
    WHERE Estado = 1;

    --tarjeta de conteo de insumos con bajo stock
    SELECT @insumosStockBajoCount = COUNT(*)
    FROM Insumo
    WHERE Estado = 1 AND Stock <= StockMinimo;

    --retornar las metricas principales para las tarjetas
    SELECT 
        @totalVentasHoy AS TotalVentasHoy,
        @ventasMesBs AS VentasMesBs,
        @totalInsumos AS TotalInsumos,
        @insumosStockBajoCount AS InsumosStockBajoCount;

    --retornar productos mas vendidos top 5
    SELECT TOP 5 
        p.Nombre AS ProductoNombre, 
        SUM(dv.Cantidad) AS Cantidad
    FROM DetalleVenta dv
    INNER JOIN Venta v ON dv.VentaId = v.Id
    INNER JOIN Producto p ON dv.ProductoId = p.Id
    WHERE v.Estado = 1 AND dv.Estado = 1
    GROUP BY p.Nombre
    ORDER BY Cantidad DESC;

    --retornar todos los metodos de pago activos con sus montos totales (incluso si son 0)
    SELECT 
        mp.Nombre AS MetodoPagoNombre, 
        ISNULL(SUM(dp.Monto), 0) AS TotalMonto
    FROM MetodoPago mp
    LEFT JOIN DetallePago dp ON mp.Id = dp.MetodoPagoId AND dp.Estado = 1
    LEFT JOIN Venta v ON dp.VentaId = v.Id AND v.Estado = 1
    WHERE mp.Estado = 1
    GROUP BY mp.Nombre
    ORDER BY TotalMonto DESC;
END
