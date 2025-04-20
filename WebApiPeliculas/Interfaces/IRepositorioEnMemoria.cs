using WebApiPeliculas.Entidades;

namespace WebApiPeliculas.Interfaces
{
    public interface IGenerosRepository
    {
        Genero obtenerGeneroPorId(int Id);
        List<Genero> retornarGeneros();
    }
}
