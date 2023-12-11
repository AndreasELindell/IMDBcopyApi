using NewApiProject.Api.Entites;

namespace NewApiProject.Api.Models
{
    public class DirectorDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
