using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApiPeliculas.Entities;

namespace WebApiPeliculas
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
    }
}
