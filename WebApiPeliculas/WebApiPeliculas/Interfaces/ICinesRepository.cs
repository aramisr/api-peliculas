using WebApiPeliculas.DTOs;
using WebApiPeliculas.Entities;

namespace WebApiPeliculas.Interfaces
{
    public interface ICinesRepository
    {
        Task<List<Cine>> GetCinesAsync(PaginacionDTO paginacionDTO, HttpContext httpContext);
        Task<Cine?> GetByIdAsync(int id);
        Task AddCineAsync(Cine cine);
        Task<bool> UpdateCineAsync(int id, Cine cineActualizado);
        Task<bool> DeleteCineAsync(int id);
    }
}
