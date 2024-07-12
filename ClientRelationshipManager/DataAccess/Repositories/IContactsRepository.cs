using BusinessLogic.Models;

namespace DataAccess.Repositories;

public interface IContactsRepository
{
    Task<IEnumerable<Contact>> GetAllAsync();
    Task<Contact> GetByIdAsync(int id);
    Task<int> AddAsync(Contact entity);
    Task<bool> UpdateAsync(Contact entity);
    Task<bool> DeleteAsync(int id);

    Task<IEnumerable<Contact>> GetAllByAdvisorIdAsync(int id);
    Task<IEnumerable<Contact>> GetAllByClientIdAsync(int id);
    
}