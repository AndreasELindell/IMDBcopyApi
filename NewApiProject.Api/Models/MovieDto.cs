using NewApiProject.Api.Entites;

namespace NewApiProject.Api.Models
{
    public class MovieDto
    {

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DirectorId { get; set; }
        public Director? Director { get; set; }

    }
}
