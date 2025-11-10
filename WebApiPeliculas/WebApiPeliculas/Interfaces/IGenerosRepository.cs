using WebApiPeliculas.DTOs;
using WebApiPeliculas.Entities;

namespace WebApiPeliculas.Interfaces
{
    public interface IGenerosRepository
    {
        Task<List<Genero>> GetGenerosAsync(PaginacionDTO paginacionDTO, HttpContext httpContext);
        Task<Genero?> GetByIdAsync(int id);
        Task AddGeneroAsync(Genero genero);
        Task<bool> UpdateGeneroAsync(int id, Genero genero);
        Task<bool> DeleteGeneroAsync(int id);
    }
}
