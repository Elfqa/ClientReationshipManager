using BusinessLogic.Models;

namespace DataAccess.Repositories;

public interface IClientsRepository
{
    //Task<IEnumerable<Client>> GetAllAsync();
    Task<Client> GetByIdAsync(int id);
    Task<IEnumerable<Client>> GetByIdAsync2(int id);
    Task<int> AddAsync(Client entity);
    Task<bool> UpdateAsync(Client entity);
    Task<bool> DeleteAsync(int id);

    Task<IEnumerable<Client>> GetAllByAdvisorIdAsync(int id);

    Task<IEnumerable<ClientWithAdvisor>> GetAllAsync();

}