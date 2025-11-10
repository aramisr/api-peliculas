using Microsoft.EntityFrameworkCore;
using WebApiPeliculas.DTOs;
using WebApiPeliculas.Entities;
using WebApiPeliculas.Interfaces;
using WebApiPeliculas.Utilidades;

namespace WebApiPeliculas.Repository
{
    public class GenerosRepository : IGenerosRepository
    {
        private readonly ApplicationDbContext _context;

        public GenerosRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Genero>> GetGenerosAsync(PaginacionDTO paginacionDTO, HttpContext httpContext)
        {
            var queryable = _context.Generos.AsQueryable();
            await httpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            return await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
        }

        public async Task<Genero?> GetByIdAsync(int id)
        {
            return await _context.Generos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddGeneroAsync(Genero genero)
        {
            _context.Add(genero);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateGeneroAsync(int id, Genero generoActualizado)
        {
            var genero = await _context.Generos.FirstOrDefaultAsync(x => x.Id == id);
            if (genero == null)
                return false;

            _context.Entry(genero).CurrentValues.SetValues(generoActualizado);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteGeneroAsync(int id)
        {
            var genero = await _context.Generos.FindAsync(id);
            if (genero == null)
                return false;

            _context.Generos.Remove(genero);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
