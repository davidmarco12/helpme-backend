﻿namespace helpme.Models
{
    public class Donacion
    {
        public int Id { get; set; } 
        public string Mensaje { get; set; } = string.Empty;
        public decimal Cantidad { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public int? ContribuyenteId { get; set; }
        public Contribuyente? Contribuyente { get; set; }
        public int? PublicacionId { get; set; }

        public bool? Estado {  get; set; }
    }
}
