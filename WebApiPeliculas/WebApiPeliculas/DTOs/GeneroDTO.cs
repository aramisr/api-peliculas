using System.ComponentModel.DataAnnotations;
using WebApiPeliculas.Validations;

namespace WebApiPeliculas.DTOs
{
    public class GeneroDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
