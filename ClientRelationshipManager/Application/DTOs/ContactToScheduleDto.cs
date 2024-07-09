namespace Application.DTOs;

public class ContactToScheduleDto
{
    public string Description { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public int AdvisorId { get; set; }
    public int ClientId { get; set; }

}