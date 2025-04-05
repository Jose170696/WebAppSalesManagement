using System.ComponentModel.DataAnnotations;

namespace WebAppSalesManagement.Models
{
    public class DetalleVenta
    {
        [Key]
        public int DetalleID { get; set; }
        public int VentaID { get; set; }
        public int ProductoID { get; set; }

        public string? NombreProducto { get; set; }
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
}
