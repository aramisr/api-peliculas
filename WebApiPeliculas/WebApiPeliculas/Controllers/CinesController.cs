using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiPeliculas.DTOs;
using WebApiPeliculas.Entities;
using WebApiPeliculas.Interfaces;
using WebApiPeliculas.Migrations;
using WebApiPeliculas.Utilidades;

namespace WebApiPeliculas.Controllers
{
    public class CinesController : ControllerBase
    {
        private readonly ILogger<CinesController> _logger;
        private readonly IMapper _mapper;
        private readonly IAlmacenadorLocal _almacenadorArchivos;
        private readonly ICinesRepository _cinesRepository;
        public readonly string contenedor = "cines";

        public CinesController(ILogger<CinesController> logger, IMapper mapper, IAlmacenadorLocal almacenadorArchivos, ICinesRepository cinesRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _almacenadorArchivos = almacenadorArchivos;
            _cinesRepository = cinesRepository;
        }

        [HttpGet("GetCines")]
        public async Task<ActionResult<List<CineDTO>>> GetCines([FromQuery] PaginacionDTO paginacionDTO)
        {
            var cines = await _cinesRepository.GetCinesAsync(paginacionDTO, HttpContext);
            return _mapper.Map<List<CineDTO>>(cines);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<ActionResult<CineDTO>> GetById(int id)
        {
            var cine = await _cinesRepository.GetByIdAsync(id);

            if (cine == null)
                return NotFound();

            return _mapper.Map<CineDTO>(cine);
        }

        [HttpPost("AddCine")]
        public async Task<ActionResult> AddActor([FromForm] CineCreacionDTO cineCreacionDTO)
        {
            var cine = _mapper.Map<Cine>(cineCreacionDTO);

            if (cineCreacionDTO.Foto != null)
            {
                cine.Foto = await _almacenadorArchivos.GuardarArchivo(contenedor, cineCreacionDTO.Foto);
            }

            await _cinesRepository.AddCineAsync(cine);
            return Ok(new { message = "Cine creado correctamente" });
        }

        [HttpPut("UpdateCine/{id:int}")]
        public async Task<ActionResult> UpdateCine(int id, [FromForm] CineCreacionDTO cineCreacionDTO)
        {
            var cineExistente = await _cinesRepository.GetByIdAsync(id);
            if (cineExistente == null)
                return NotFound();

            _mapper.Map(cineCreacionDTO, cineExistente);

            if (cineCreacionDTO.Foto != null)
            {
                cineExistente.Foto = await _almacenadorArchivos.EditarArchivo(contenedor, cineExistente.Foto, cineCreacionDTO.Foto);
            }

            await _cinesRepository.UpdateCineAsync(id, cineExistente);
            return NoContent();
        }

        [HttpDelete("DeleteCine/{id:int}")]
        public async Task<ActionResult> DeleteActor(int id)
        {
            var cine = await _cinesRepository.GetByIdAsync(id);
            if (cine == null)
                return NotFound();

            if (!string.IsNullOrEmpty(cine.Foto))
            {
                await _almacenadorArchivos.BorrarArchivo(contenedor, cine.Foto);
            }

            await _cinesRepository.DeleteCineAsync(id);
            return NoContent();
        }
    }
}
