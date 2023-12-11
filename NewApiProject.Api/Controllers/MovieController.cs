using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        const int maxPageSize = 20;
        public MovieController(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieDto>>> GetAllMovies(
            [FromQuery] string? title, string? searchQuery, int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }

            var (movieEntities, paginationMetaData) = await _movieRepository
                .GetMoviesAsync(title, searchQuery, pageNumber, pageSize);

            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(paginationMetaData);
            return Ok(_mapper.Map<IEnumerable<MovieDto>>(movieEntities));

        }

        [HttpGet("{movieId}", Name = "GetMovie")]
        public async Task<ActionResult<MovieDto>> GetMovie(int movieId)
        {
            if (!await _movieRepository.MovieExitsAsync(movieId))
            {
                return NotFound();
            }
            var movie = await _movieRepository.GetMovieAsync(movieId);

            return Ok(_mapper.Map<MovieDto>(movie));

        }

        [HttpPost]
        public async Task<ActionResult<MovieDto>> AddMovie(MovieCreationDto movie, int directorId)
        {
            if (!await _movieRepository.DirectorExitsAsync(directorId))
            {
                return NotFound();
            }

            var mappedMovieToAdd = _mapper.Map<Movie>(movie);

            await _movieRepository.AddMovieAsync(mappedMovieToAdd, directorId);

            await _movieRepository.SaveChangesAsync();

            var createdMovieToReturn = _mapper.Map<MovieDto>(mappedMovieToAdd);

            return CreatedAtRoute("GetMovie", new { movieId = createdMovieToReturn.Id }, createdMovieToReturn);
        }
    }
}
