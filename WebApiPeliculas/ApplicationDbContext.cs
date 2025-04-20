using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebApiPeliculas.Entidades;

namespace WebApiPeliculas
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Genero> Generos { get; set; }
    }
}
