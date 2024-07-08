namespace BusinessLogic.Models;

/// <summary>
/// The business model of the user - customer advisor, after reading the user from the database - in the context of contact management
/// </summary>
public class UserAdvisor
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    //public string Password { get; set; }
    public IEnumerable<Client>? Clients { get; set; }
    public IEnumerable<Contact>? Contacts { get; set; }

 
}