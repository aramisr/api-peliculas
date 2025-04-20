using AutoMapper;
using WebApiPeliculas.DTOs;
using WebApiPeliculas.Entidades;

namespace WebApiPeliculas.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<Genero, GeneroCreacionDTO>().ReverseMap();
        }
    }
}
