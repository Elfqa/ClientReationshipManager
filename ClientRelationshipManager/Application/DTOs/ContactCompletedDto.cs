using BusinessLogic.Models;

namespace Application.DTOs;

public class ContactCompletedDto
{
    public string Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}