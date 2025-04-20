using WebApiPeliculas.Entidades;
using WebApiPeliculas.Interfaces;

namespace WebApiPeliculas.Repository
{
    public class GenerosRepository : IGenerosRepository
    {
        private List<Genero> _generos;

        public GenerosRepository()
        {
            _generos = new List<Genero>()
            {
                new Genero { Id=1, Nombre="Accion" },
                new Genero { Id = 2, Nombre = "Romance" },
                new Genero { Id = 3, Nombre = "Crimen" }
            };
        }

        public List<Genero> retornarGeneros()
        {
            return _generos;
        }

        public Genero obtenerGeneroPorId(int Id)
        {
            return _generos.FirstOrDefault(x => x.Id == Id);
        }
    }
}
