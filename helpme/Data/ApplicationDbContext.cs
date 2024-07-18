using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using helpme.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<helpme.Models.Contribuyente> Contribuyente { get; set; } = default!;

        public DbSet<helpme.Models.Organizacion> Organizacion { get; set; } = default!;

        public DbSet<helpme.Models.Donacion> Donacion { get; set; } = default!;

        public DbSet<helpme.Models.Publicacion> Publicacion { get; set; } = default!;

        public DbSet<helpme.Models.Usuario> Usuario { get; set; } = default!;
    }
