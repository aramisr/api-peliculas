using Microsoft.EntityFrameworkCore;
using WebApiPeliculas.DTOs;
using WebApiPeliculas.Entities;
using WebApiPeliculas.Interfaces;
using WebApiPeliculas.Utilidades;

namespace WebApiPeliculas.Repository
{
    public class CinesRepository : ICinesRepository
    {
        private readonly ApplicationDbContext _context;

        public CinesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cine>> GetCinesAsync(PaginacionDTO paginacionDTO, HttpContext httpContext)
        {
            var queryable = _context.Cines.AsQueryable();
            await httpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            return await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
        }

        public async Task<Cine?> GetByIdAsync(int id)
        {
            return await _context.Cines.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddCineAsync(Cine cine)
        {
            _context.Add(cine);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateCineAsync(int id, Cine cineActualizado)
        {
            var cine = await _context.Cines.FirstOrDefaultAsync(x => x.Id == id);
            if (cine == null)
                return false;

            _context.Entry(cine).CurrentValues.SetValues(cineActualizado);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCineAsync(int id)
        {
            var cine = await _context.Cines.FindAsync(id);
            if (cine == null)
                return false;

            _context.Cines.Remove(cine);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
