using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiPeliculas.DTOs;
using WebApiPeliculas.Entities;
using WebApiPeliculas.Utilidades;

namespace WebApiPeliculas.Controllers
{
    [Route("api/actores")]
    [ApiController]
    public class ActoresController : ControllerBase
    {
        private readonly ILogger<GenerosController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAlmacenadorLocal _almacenadorArchivos;
        public readonly string contenedor = "actores";

        public ActoresController(ILogger<GenerosController> logger, ApplicationDbContext dbContext, IMapper mapper, IAlmacenadorLocal almacenadorArchivos)
        {
            this._logger = logger;
            this._dbContext = dbContext;
            this._mapper = mapper;
            this._almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet("GetActores")]
        public async Task<ActionResult<List<ActorDTO>>> GetActores([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = _dbContext.Actores.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var actores = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
            return _mapper.Map<List<ActorDTO>>(actores);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<ActorDTO>> GetById(int id)
        {
            var actor = await _dbContext.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (actor == null)
            {
                return NotFound();
            }
            return _mapper.Map<ActorDTO>(actor);
        }

        [HttpPost("AddActor")]
        public async Task<ActionResult> AddActor([FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actor = _mapper.Map<Actor>(actorCreacionDTO);

            if(actorCreacionDTO.Foto != null)
            {
                actor.Foto = await _almacenadorArchivos.GuardarArchivo(contenedor, actorCreacionDTO.Foto);
            }

            _dbContext.Add(actor);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("UpdateActor/{id:int}")]
        public async Task<ActionResult> UpdateActor(int id, [FromForm] ActorCreacionDTO actorCreacionDTO)
        {
            var actor = await _dbContext.Actores.FirstOrDefaultAsync(x => x.Id == id);

            if (actor == null)
            {
                return NotFound();
            }
            actor = _mapper.Map(actorCreacionDTO, actor);

            if (actorCreacionDTO.Foto != null)
            {
                actor.Foto = await _almacenadorArchivos.EditarArchivo(contenedor, actor.Foto, actorCreacionDTO.Foto);
            }

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("DeleteActor/{id:int}")]
        public async Task<ActionResult> DeleteActor(int id)
        {
            var existe = await _dbContext.Actores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            _dbContext.Remove(new Actor() { Id = id });
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
