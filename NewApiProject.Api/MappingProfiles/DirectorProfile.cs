using AutoMapper;
using NewApiProject.Api.Entites;
using NewApiProject.Api.Models;

namespace NewApiProject.Api.MappingProfiles
{
    public class DirectorProfile : Profile
    {
        public DirectorProfile()
        {
            CreateMap<Director, DirectorDto>();
            CreateMap<DirectorDto, Director>();
            CreateMap<DirectorCreationDto, Director>();
        }
    }
}
