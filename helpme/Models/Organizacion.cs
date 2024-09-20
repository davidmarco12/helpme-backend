namespace helpme.Models
{
    public class Organizacion : Usuario
    {
        public string NombreOrganizacion { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string CUIT { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
        public string Provincia {  get; set; } = string.Empty;
        public DateTime? FechaDeCreacion { get; set; }
        public List<Publicacion> Publicaciones { get; set; } = new List<Publicacion>();
        public int IdUsuario { get; set; }
        public Usuario? Usuario { get; set; }  
    }
}
