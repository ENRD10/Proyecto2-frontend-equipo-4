namespace PosManagerApp.Models.DTOs
{
    public class CreateVentaRequest
    {
        public int IdCliente { get; set; }
        public int IdEmpleado { get; set; }
        public DateTime FechaOrden { get; set; }
        public decimal Impuesto { get; set; }
        public List<CreateDetalleVentaRequest> Detalles { get; set; } = new();
    }

    public class CreateDetalleVentaRequest
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; }
        public decimal Impuesto { get; set; }
    }

    // DTO de respuesta
    public class VentaResponse
    {
        public int NumeroOrden { get; set; }
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; } = string.Empty;
        public DateTime FechaOrden { get; set; }
        public decimal Impuesto { get; set; }
        public decimal TotalOrden { get; set; }
        public List<DetalleVentaResponse> Detalles { get; set; } = new();
    }

    public class DetalleVentaResponse
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; }
        public decimal Impuesto { get; set; }
        public decimal PrecioSubtotal { get; set; }
        public decimal PrecioTotal { get; set; }
    }

    public class CreateVentaResponse
    {
        public int NumeroOrden { get; set; }

        public string Mensaje { get; set; } = string.Empty;
    }
}