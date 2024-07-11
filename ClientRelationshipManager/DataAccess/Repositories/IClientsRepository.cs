using BusinessLogic.Models;

namespace DataAccess.Repositories;

public interface IClientsRepository
{
    Task<IEnumerable<ClientWithAdvisor>> GetAllAsync();
    Task<ClientWithAdvisor> GetByIdAsync(int id);
    Task<int> AddAsync(ClientWithAdvisor entity);
    Task<bool> UpdateAsync(ClientWithAdvisor entity);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ClientWithAdvisor>> GetAllByAdvisorIdAsync(int id);

}