using System;
using System.ComponentModel.DataAnnotations;

namespace WebAppSalesManagement.Models
{
    public class ProductoViewModel
    {
        public int ProductoID { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que 0")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public decimal Stock { get; set; }

        public string? AdicionadoPor { get; set; }
        public DateTime? FechaAdicion { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
