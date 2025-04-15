using System.ComponentModel.DataAnnotations;

namespace WebAppSalesManagement.Models
{
    public class ClienteViewModel
    {
        public int ClienteID { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Required]
        [Phone]
        public string Telefono { get; set; }

        public string Pais { get; set; }
        public string Provincia { get; set; }
        public string Canton { get; set; }
        public string Distrito { get; set; }

        public string? AdicionadoPor { get; set; }
        public DateTime? FechaAdicion { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}