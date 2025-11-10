using Microsoft.AspNetCore.Mvc;
using WebApiPeliculas.Entities;
using WebApiPeliculas.Interfaces;
using WebApiPeliculas.DTOs;
using AutoMapper;
using WebApiPeliculas.Utilidades;

namespace WebApiPeliculas.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private readonly ILogger<GenerosController> _logger;
        private readonly IMapper _mapper;
        private readonly IGenerosRepository _generosRepository;

        public GenerosController(
            ILogger<GenerosController> logger,
            IMapper mapper,
            IGenerosRepository generosRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _generosRepository = generosRepository;
        }

        [HttpGet("GetGeneros")]
        public async Task<ActionResult<List<GeneroDTO>>> GetGeneros([FromQuery] PaginacionDTO paginacionDTO)
        {
            var generos = await _generosRepository.GetGenerosAsync(paginacionDTO, HttpContext);
            return _mapper.Map<List<GeneroDTO>>(generos);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<GeneroDTO>> GetById(int id)
        {
            var genero = await _generosRepository.GetByIdAsync(id);
            if (genero == null)
                return NotFound();

            return _mapper.Map<GeneroDTO>(genero);
        }

        [HttpPost("AddGenero")]
        public async Task<ActionResult> AddGenero([FromForm] GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = _mapper.Map<Genero>(generoCreacionDTO);
            await _generosRepository.AddGeneroAsync(genero);
            return Ok(new { message = "Género creado correctamente" });
        }

        [HttpPut("UpdateGenero/{id:int}")]
        public async Task<ActionResult> UpdateGenero(int id, [FromForm] GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = _mapper.Map<Genero>(generoCreacionDTO);
            var actualizado = await _generosRepository.UpdateGeneroAsync(id, genero);

            if (!actualizado)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("DeleteGenero/{id:int}")]
        public async Task<ActionResult> DeleteGenero(int id)
        {
            var eliminado = await _generosRepository.DeleteGeneroAsync(id);

            if (!eliminado)
                return NotFound();

            return NoContent();
        }
    }
}
