using BusinessLogic.Models;

namespace Application.Services;

public interface IUserAccountService
{
    Task<UserAccount?> Authenticate(string name, string password);

}