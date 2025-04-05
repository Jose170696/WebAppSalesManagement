using System.ComponentModel.DataAnnotations;

namespace WebAppSalesManagement.Models
{
    public class Venta
    {
        [Key]
        public int VentaID { get; set; }
        public int ClienteID { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string AdicionadoPor { get; set; }
        public DateTime FechaAdicion { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }

        public Cliente Cliente { get; set; }
    }
}
