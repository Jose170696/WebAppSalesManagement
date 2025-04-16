using System.ComponentModel.DataAnnotations;

namespace WebAppSalesManagement.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioID { get; set; }
        [Required]
        public string NombreUsuario { get; set; }
        [Required]
        public string Contraseña { get; set; }
        [Required]
        public string NombreCompleto { get; set; }
        [Required]
        [EmailAddress]
        public string Correo { get; set; }
        public int RolID { get; set; }
        public string Estado { get; set; }
        public string? RolNombre { get; set; }

        public Roles Rol { get; set; }
    }
}
