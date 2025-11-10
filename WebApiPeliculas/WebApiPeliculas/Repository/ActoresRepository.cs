using Microsoft.EntityFrameworkCore;
using WebApiPeliculas.DTOs;
using WebApiPeliculas.Entities;
using WebApiPeliculas.Interfaces;
using WebApiPeliculas.Utilidades;

namespace WebApiPeliculas.Repository
{
    public class ActoresRepository : IActoresRepository
    {
        private readonly ApplicationDbContext _context;

        public ActoresRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Actor>> GetActoresAsync(PaginacionDTO paginacionDTO, HttpContext httpContext)
        {
            var queryable = _context.Actores.AsQueryable();
            await httpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            return await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
        }

        public async Task<Actor?> GetByIdAsync(int id)
        {
            return await _context.Actores.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddActorAsync(Actor actor)
        {
            _context.Add(actor);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateActorAsync(int id, Actor actorActualizado)
        {
            var actor = await _context.Actores.FirstOrDefaultAsync(x => x.Id == id);
            if (actor == null)
                return false;

            _context.Entry(actor).CurrentValues.SetValues(actorActualizado);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteActorAsync(int id)
        {
            var actor = await _context.Actores.FindAsync(id);
            if (actor == null)
                return false;

            _context.Actores.Remove(actor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
