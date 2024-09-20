using System.ComponentModel.DataAnnotations;

namespace helpme.Models
{
    public class Contribuyente : Usuario
    {
        public string? Nombre { get; set; } = string.Empty;
        public string? Apellido { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }
        public int? IdUsuario { get; set; }
        public Usuario? Usuario { get; set; }   
    }
}
