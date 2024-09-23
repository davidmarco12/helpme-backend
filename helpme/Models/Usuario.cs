using System.ComponentModel.DataAnnotations;

namespace helpme.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string? User { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PasswordHash { get; set; } = string.Empty;
        public string? TipoDeUsuario { get; set; } = string.Empty;
        public string? UrlImagen { get; set; } = "https://img.freepik.com/free-vector/blue-circle-with-white-user_78370-4707.jpg";
    }
}
