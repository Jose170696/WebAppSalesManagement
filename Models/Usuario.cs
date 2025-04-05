using System.ComponentModel.DataAnnotations;

namespace WebAppSalesManagement.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public string ContraseñaHash { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public int RolID { get; set; }
        public string Estado { get; set; }
        public string? RolNombre { get; set; }

        public Roles Rol { get; set; }
    }
}
