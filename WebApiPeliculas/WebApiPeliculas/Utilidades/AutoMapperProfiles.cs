using AutoMapper;
using NetTopologySuite.Geometries;
using WebApiPeliculas.DTOs;
using WebApiPeliculas.Entities;

namespace WebApiPeliculas.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(GeometryFactory geomtryFactory) 
        {
            CreateMap<Genero, GeneroDTO>().ReverseMap();
            CreateMap<GeneroCreacionDTO, Genero>();
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<ActorCreacionDTO, Actor>()
                .ForMember(x => x.Foto, options => options.Ignore());
            CreateMap<CineCreacionDTO, Cine>()
               .ForMember(x => x.Ubicacion, x => x.MapFrom(dto =>
               geomtryFactory.CreatePoint(new Coordinate(dto.Longitud, dto.Latitud))));
        }
    }
}
