using Microsoft.EntityFrameworkCore;
using NewApiProject.Api.DbContext;
using NewApiProject.Api.Entites;
using NewApiProject.Api.Services;

namespace NewApiProject.Api.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieContext _context;
        public MovieRepository(MovieContext context)
        {
            _context = context;
        }

        public async Task AddMovieAsync(Movie movie, int directorId)
        {
            var directorExists = await DirectorExitsAsync(directorId);
            if (directorExists)
            {
                movie.DirectorId = directorId;
                _context.Movies.Add(movie);
            }

        }
        public void AddDirector(Director director)
        {
            _context.Directors.Add(director);
        }

        public void DeleteMovieAsync(int movieId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DirectorExitsAsync(int directorId)
        {
            return await _context.Directors.AnyAsync(d => d.Id == directorId);
        }

        public async Task<Director?> GetDirectorAsync(int directorId)
        {
            return await _context.Directors.FirstOrDefaultAsync(d => d.Id == directorId);
        }

        public async Task<(IEnumerable<Director>, PaginationMetaData)> GetDirectorsAsync(string? searchQuery, int pageNumber, int pageSize)
        {
            var collection = _context.Directors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(d =>
                d.FirstName.Contains(searchQuery) ||
                d.LastName.Contains(searchQuery));
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetaData = new PaginationMetaData(totalItemCount, pageSize, pageNumber);


            var collectionToReturn = await collection
                .OrderBy(d => d.LastName)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize).ToListAsync();
            return (collectionToReturn, paginationMetaData);
        }

        public async Task<Movie?> GetMovieAsync(int movieId)
        {
            return await _context.Movies.Include(d => d.Director).FirstOrDefaultAsync(movie => movie.Id == movieId);
        }

        public async Task<(IEnumerable<Movie>, PaginationMetaData)> GetMoviesAsync(string? title, string? searchQuery, int pageNumber, int pageSize)
        {
            var collection = _context.Movies.Include(d => d.Director).AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                title = title.Trim();
                collection.Where(m => m.Title == title);
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(m => m.Title.Contains(searchQuery)
                || (m.Description != null && m.Description.Contains(searchQuery)));
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetaData = new PaginationMetaData(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(c => c.Title)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetaData);
        }

        public async Task<bool> MovieExitsAsync(int movieId)
        {
            return await _context.Movies.AnyAsync(movie => movie.Id == movieId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
