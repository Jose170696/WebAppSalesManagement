using System.ComponentModel.DataAnnotations;

namespace WebAppSalesManagement.Models
{
    public class Venta
    {
        public int VentaID { get; set; }
        public int ClienteID { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string AdicionadoPor { get; set; }
        public DateTime FechaAdicion { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }

    public class VentaCompleta
    {
        public int VentaID { get; set; }
        public int ClienteID { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string AdicionadoPor { get; set; }
        public DateTime FechaAdicion { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public List<DetalleVenta> Detalles { get; set; } = new();
    }

    public class DetalleVenta
    {
        public int DetalleID { get; set; }
        public int VentaID { get; set; }
        public int ProductoID { get; set; }
        public string NombreProducto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Subtotal { get; set; }
        public string AdicionadoPor { get; set; }
        public DateTime FechaAdicion { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }

    public class DetalleVentaItem
    {
        public int ProductoID { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class VentaConDetalles
    {
        public int ClienteID { get; set; }
        public decimal Total { get; set; }
        public string AdicionadoPor { get; set; }
        public List<DetalleVentaItem> Detalles { get; set; } = new List<DetalleVentaItem>();
    }
}
