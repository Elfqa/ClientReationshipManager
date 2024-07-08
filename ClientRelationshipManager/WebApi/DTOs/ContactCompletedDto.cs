using BusinessLogic.Models;

namespace WebApi.DTOs;

public class ContactCompletedDto
{
    public string Description { get; set; }

    //public DateTime? ScheduledDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    //public ContactStatus Status { get; set; } = ContactStatus.Completed;
    //public int AdvisorId { get; set; }
    //public int ClientId { get; set; }
    //public UserAdvisor Advisor { get; set; }
    //public Client Client { get; set; }
}