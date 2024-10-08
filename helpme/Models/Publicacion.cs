﻿using System.Text.Json.Serialization;

namespace helpme.Models
{
    public class Publicacion
    {
        public int Id { get; set; }
        public string? Titulo { get; set; } = string.Empty;
        public string? Contenido { get; set; } = string.Empty;
        public string? UrlImagenes {  get; set; } = string.Empty;
        public DateTime? FechaCreacion { get; set; }
        public int? OrganizacionId { get; set; }
        public string? DescripcionDonacion { get; set; }
        public string? ReferenciaDePago { get; set; }

        [JsonIgnore]
        public Organizacion? Organizacion { get; set;}
        public List<Imagen>? Imagenes { get; set; } = new List<Imagen>();
    }
}
