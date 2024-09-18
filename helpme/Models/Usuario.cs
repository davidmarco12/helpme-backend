using System.ComponentModel.DataAnnotations;

namespace helpme.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string User { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string TipoDeUsuario { get; set; } = string.Empty;
    }
}
