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
using Microsoft.Extensions.FileProviders;

namespace WebApiPeliculas.Controllers
{
    [Route("api/generos")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private readonly ILogger<GenerosController> logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public GenerosController(ILogger<GenerosController> logger, ApplicationDbContext dbContext, IMapper mapper)
        {
            this.logger = logger;
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        [HttpGet("GetGeneros")]
        public async Task<ActionResult<List<GeneroDTO>>> GetGeneros([FromQuery] PaginacionDTO paginacionDTO)
        {
            var queryable = _dbContext.Generos.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var generos = await queryable.OrderBy(x => x.Nombre).Paginar(paginacionDTO).ToListAsync();
            return _mapper.Map<List<GeneroDTO>>(generos);
        }

        [HttpGet("GetById/{Id:int}")]
        public async Task<ActionResult<GeneroDTO>> GetById(int Id)
        {
            var genero = await _dbContext.Generos.FirstOrDefaultAsync(x => x.Id == Id);
            
            if(genero == null)
            {
                return NotFound();
            }
            return _mapper.Map<GeneroDTO>(genero);
        }

        [HttpPost("AddGenero")]
        public async Task<ActionResult> AddGenero([FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = _mapper.Map<Genero>(generoCreacionDTO);
            _dbContext.Add(genero);
            await _dbContext.SaveChangesAsync();
            return Ok(new { message = "Género creado correctamente" });
        }

        [HttpPut("UpdateGenero")]
        public async Task<ActionResult> UpdateGenero(int id, [FromBody] GeneroCreacionDTO generoCreacionDTO)
        {
            var genero = await _dbContext.Generos.FirstOrDefaultAsync(x => x.Id == id);

            if (genero == null)
            {
                return NotFound();
            }
            genero = _mapper.Map(generoCreacionDTO, genero);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await _dbContext.Generos.AnyAsync(x => x.Id == id);

            if(!existe){
                return NotFound();
            }

            _dbContext.Remove(new Genero() { Id = id });
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
