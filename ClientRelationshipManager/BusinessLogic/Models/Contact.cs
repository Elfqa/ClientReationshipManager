namespace BusinessLogic.Models;

public class Contact
{
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdate { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ContactStatus Status { get; set; }
    public int AdvisorId { get; set; }
    public int ClientId { get; set; }
    //public UserAdvisor Advisor { get; set; }
    //public Client Client { get; set; }
}