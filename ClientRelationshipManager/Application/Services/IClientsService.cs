using Application.DTOs;
using BusinessLogic.Models;

namespace Application.Services;

public interface IClientsService
{
    Task<IEnumerable<ClientWithAdvisor>> GetAllClientsAsync();
    Task<ClientWithAdvisor> GetClientByIdAsync(int id);
    Task<IEnumerable<ClientWithAdvisor>?> GetAllClientsByAdvisorIdAsync(int id);
    Task<int?> AddClientAsync(NewClientDto newClientDto);
    Task<bool?> UpdateClientAsync(int id, ClientDto clientDto);
    Task<bool> DeleteClientAsync(int id);
}