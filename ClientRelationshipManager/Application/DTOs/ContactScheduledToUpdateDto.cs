using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class ContactScheduledToUpdateDto
{
    public string Description { get; set; }
    [Required]
    public DateTime ScheduledDate { get; set; }


}