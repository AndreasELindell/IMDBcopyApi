using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewApiProject.Api.Entites;
using NewApiProject.Api.Models;
using NewApiProject.Api.Repositories;
using System.Text.Json;

namespace NewApiProject.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        const int maxPageSize = 20;

        public DirectorController(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DirectorDto>>> GetAllDirectors(
            [FromQuery]string? searchQuery, int pageNumber = 1, int pageSize = 10) 
        {
            if(pageSize > maxPageSize) 
            { 
                pageSize = maxPageSize;
            }

            var (directorsEnties, paginationMetaData) = await _movieRepository
                .GetDirectorsAsync(searchQuery, pageNumber, pageSize);

            Response.Headers["X-pagination"] = JsonSerializer.Serialize(paginationMetaData);

            return Ok(_mapper.Map<IEnumerable<DirectorDto>>(directorsEnties));
        }
        [HttpGet("{directorId}", Name = "GetDirector")]
        public async Task<ActionResult<DirectorDto>> GetDirector(int directorId)
        {
            if (!await _movieRepository.DirectorExitsAsync(directorId))
            {
                return NotFound();
            }

            var director = await _movieRepository.GetDirectorAsync(directorId);

            return Ok(_mapper.Map<DirectorDto>(director));
        }
        [HttpPost]
        public async Task<ActionResult<DirectorDto>> CreateDirector(DirectorCreationDto directorCreationDto) 
        { 
            var mappedDirectorToAdd = _mapper.Map<Director>(directorCreationDto);

            _movieRepository.AddDirector(mappedDirectorToAdd);

            await _movieRepository.SaveChangesAsync();

            var directorToReturn = _mapper.Map<DirectorDto>(mappedDirectorToAdd);

            return CreatedAtRoute("GetDirector", new { directorId = directorToReturn.Id }, directorToReturn);
        }
    }
}
