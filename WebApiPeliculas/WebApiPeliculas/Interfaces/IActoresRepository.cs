using WebApiPeliculas.DTOs;
using WebApiPeliculas.Entities;

namespace WebApiPeliculas.Interfaces
{
    public interface IActoresRepository
    {
        Task<List<Actor>> GetActoresAsync(PaginacionDTO paginacionDTO, HttpContext httpContext);
        Task<Actor?> GetByIdAsync(int id);
        Task AddActorAsync(Actor actor);
        Task<bool> UpdateActorAsync(int id, Actor actorActualizado);
        Task<bool> DeleteActorAsync(int id);
    }
}
