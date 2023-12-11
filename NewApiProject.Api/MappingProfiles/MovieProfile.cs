using AutoMapper;
using NewApiProject.Api.Entites;
using NewApiProject.Api.Models;

namespace NewApiProject.Api.MappingProfiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieDto>();
            CreateMap<MovieDto, Movie>();
            CreateMap<MovieCreationDto, Movie>();
        }
    }
}
