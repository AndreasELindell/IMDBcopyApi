using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewApiProject.Api.Entites
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }

        [ForeignKey("DirectorId")]
        public Director? Director { get; set; } 
        public int DirectorId { get; set; }

        public Movie(string title)
        {
            Title = title;
        }
    }
}
