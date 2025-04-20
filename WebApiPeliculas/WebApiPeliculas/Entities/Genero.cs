using System.ComponentModel.DataAnnotations;
using WebApiPeliculas.Validations;

namespace WebApiPeliculas.Entidades
{
    public class Genero
    {
        [Required]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido ")]
        [StringLength(maximumLength:10)]
        [PrimeraLetraMayusculaAtribute]
        public string Nombre { get; set; }
    }
}
