namespace BusinessLogic.Models;

public class Client
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public UserAdvisor? Advisor { get; set; }
    public IEnumerable<Contact>? Contacts { get; set; }
}

public class ClientWithAdvisor
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int AdvisorId { get; set; }
    public string AdvisorName { get; set; }
    
}
public class AdvisorInClientCard
{
    public int Id { get; set; }
    public string Name { get; set; }

}