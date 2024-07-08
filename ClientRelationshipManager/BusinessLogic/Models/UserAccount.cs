namespace BusinessLogic.Models;

/// <summary>
/// System user model for authentication. Business and database model of the account in the system.
/// </summary>
public class UserAccount
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}