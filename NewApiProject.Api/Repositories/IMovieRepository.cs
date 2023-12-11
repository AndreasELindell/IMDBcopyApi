using NewApiProject.Api.Entites;
using NewApiProject.Api.Services;

namespace NewApiProject.Api.Repositories
{
    public interface IMovieRepository
    {
        Task<(IEnumerable<Movie>, PaginationMetaData)> GetMoviesAsync(string? title, string? searchQuery, int pageNumber, int pageSize);
        Task<Movie?> GetMovieAsync(int movieId);
        Task<(IEnumerable<Director>, PaginationMetaData)> GetDirectorsAsync(string? searchQuery, int pageNumber, int pageSize);
        Task<Director?> GetDirectorAsync(int directorId);
        Task<bool> MovieExitsAsync(int movieId);
        Task<bool> DirectorExitsAsync(int directorId);
        Task AddMovieAsync(Movie movie, int directorId);
        void DeleteMovieAsync(int movieId);
        Task<bool> SaveChangesAsync();
        void AddDirector(Director director);
    }
}
