using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class NewContactToScheduleDto
{
    public string Description { get; set; }
    [Required]
    public DateTime ScheduledDate { get; set; }
    [Required]
    public int AdvisorId { get; set; }
    [Required]
    public int ClientId { get; set; }

}