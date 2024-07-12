using BusinessLogic.Models;

namespace DataAccess.Repositories;

public interface IUserAccountRepository
{
    Task<IEnumerable<UserAccount>> GetAllAsync();
    Task<UserAccount?> Authenticate(string name, string password);

}