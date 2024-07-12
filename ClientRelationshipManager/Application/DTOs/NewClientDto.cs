using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class NewClientDto
{
    [Required(AllowEmptyStrings = false)]
    public string FirstName { get; set; }
    [Required(AllowEmptyStrings = false)]
    public string LastName { get; set; }
    [Required]
    public int AdvisorId { get; set; }
}