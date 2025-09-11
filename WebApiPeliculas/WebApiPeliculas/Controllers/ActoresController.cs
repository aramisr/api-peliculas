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
    }
}
