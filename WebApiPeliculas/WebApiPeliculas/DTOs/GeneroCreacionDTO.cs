using System.ComponentModel.DataAnnotations;
using WebApiPeliculas.Validations;

namespace WebApiPeliculas.DTOs
{
    public class GeneroCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido ")]
        [StringLength(maximumLength: 10)]
        [PrimeraLetraMayusculaAtribute]
        public string Nombre { get; set; }
    }
}
