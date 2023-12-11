using System.ComponentModel.DataAnnotations;

namespace NewApiProject.Api.Models;

public class DirectorCreationDto
{
    [Required(ErrorMessage = "You need to submit a First Name")]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    [Required(ErrorMessage = "You need to submit a Last Name")]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
}


