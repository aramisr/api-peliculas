using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using WebApiPeliculas.Entidades;
using WebApiPeliculas.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApiPeliculas.Filtros;
using Microsoft.EntityFrameworkCore;
using WebApiPeliculas.DTOs;
using AutoMapper;
using WebApiPeliculas.Utilidades;

namespace WebApiPeliculas.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private readonly ILogger<GenerosController> logger;
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;
        public GenerosController(ILogger<GenerosController> logger, ApplicationDbContext dbContext, IMapper mapper)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet("GetGeneros")]
        public async Task<ActionResult<List<GeneroDTO>>> Get([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = dbContext.Generos.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var generos = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
            return mapper.Map<List<GeneroDTO>>(generos);
        }

        [HttpGet("GetById/{Id:int}")]
        public async Task<ActionResult<GeneroDTO>> GetById(int Id)
        {
            var genero = await dbContext.Generos.FirstOrDefaultAsync(x => x.Id == Id);
            
            if(genero == null)
            {
                return NotFound();
            }
            return mapper.Map<GeneroDTO>(genero);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = mapper.Map<Genero>(generoCreacionDTO);
            dbContext.Add(genero);
            await dbContext.SaveChangesAsync();
            return Ok(new { message = "Género creado correctamente" });
        }

        [HttpPut("EditarGenero")]
        public async Task<ActionResult> EditarGenero(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = await dbContext.Generos.FirstOrDefaultAsync(x => x.Id == id);

            if (genero == null)
            {
                return NotFound();
            }
            genero = mapper.Map(generoCreacionDTO, genero);
            await dbContext.SaveChangesAsync();
            return NoContent();

        }
        [HttpDelete]
        public async Task<ActionResult> Delete()
        {
            throw new NotImplementedException();
        }
    }
}
