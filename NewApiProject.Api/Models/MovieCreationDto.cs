using System.ComponentModel.DataAnnotations;

namespace NewApiProject.Api.Models
{
    public class MovieCreationDto
    {
        [Required(ErrorMessage = "You need to provied a Title")]
        [MaxLength(50)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
