using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApiPeliculas.DTOs;
using WebApiPeliculas.Entities;
using WebApiPeliculas.Interfaces;
using WebApiPeliculas.Utilidades;

namespace WebApiPeliculas.Controllers
{
    [Route("api/actores")]
    [ApiController]
    public class ActoresController : ControllerBase
    {
        private readonly ILogger<ActoresController> _logger;
        private readonly IMapper _mapper;
        private readonly IAlmacenadorLocal _almacenadorArchivos;
        private readonly IActoresRepository _actoresRepository;
        public readonly string contenedor = "actores";

        public ActoresController(ILogger<ActoresController> logger, IMapper mapper, IAlmacenadorLocal almacenadorArchivos, IActoresRepository actoresRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _almacenadorArchivos = almacenadorArchivos;
            _actoresRepository = actoresRepository;
        }

        [HttpGet("GetActores")]
        public async Task<ActionResult<List<ActorDTO>>> GetActores([FromQuery] PaginacionDTO paginacionDTO)
        {
            var actores = await _actoresRepository.GetActoresAsync(paginacionDTO, HttpContext);
            return _mapper.Map<List<ActorDTO>>(actores);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<ActorDTO>> GetById(int id)
        {
            var actor = await _actoresRepository.GetByIdAsync(id);

            if (actor == null)
                return NotFound();

            return _mapper.Map<ActorDTO>(actor);
        }

        [HttpPost("AddActor")]
        public async Task<ActionResult> AddActor([FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actor = _mapper.Map<Actor>(actorCreacionDTO);

            if (actorCreacionDTO.Foto != null)
            {
                actor.Foto = await _almacenadorArchivos.GuardarArchivo(contenedor, actorCreacionDTO.Foto);
            }

            await _actoresRepository.AddActorAsync(actor);
            return Ok(new { message = "Actor creado correctamente" });
        }

        [HttpPut("UpdateActor/{id:int}")]
        public async Task<ActionResult> UpdateActor(int id, [FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actorExistente = await _actoresRepository.GetByIdAsync(id);
            if (actorExistente == null)
                return NotFound();

            _mapper.Map(actorCreacionDTO, actorExistente);

            if (actorCreacionDTO.Foto != null)
            {
                actorExistente.Foto = await _almacenadorArchivos.EditarArchivo(contenedor, actorExistente.Foto, actorCreacionDTO.Foto);
            }

            await _actoresRepository.UpdateActorAsync(id, actorExistente);
            return NoContent();
        }

        [HttpDelete("DeleteActor/{id:int}")]
        public async Task<ActionResult> DeleteActor(int id)
        {
            var actor = await _actoresRepository.GetByIdAsync(id);
            if (actor == null)
                return NotFound();

            if (!string.IsNullOrEmpty(actor.Foto))
            {
                await _almacenadorArchivos.BorrarArchivo(contenedor, actor.Foto);
            }

            await _actoresRepository.DeleteActorAsync(id);
            return NoContent();
        }
    }
}
