using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Models;
using DataAccess.Repositories;
using Application.DTOs;

namespace Application.Services
{
    public class ClientsService : IClientsService
    {
        private readonly IClientsRepository _repository;

        public ClientsService(IClientsRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ClientWithAdvisor>> GetAllClientsAsync()
        {
            var clients = await _repository.GetAllAsync();
            return clients;
        }

        public async Task<ClientWithAdvisor> GetClientByIdAsync(int id)
        {
            var client = await _repository.GetByIdAsync(id);
            return client;
        }

        public async Task<IEnumerable<ClientWithAdvisor>?> GetAllClientsByAdvisorIdAsync(int id)
        {
            var clients = await _repository.GetAllByAdvisorIdAsync(id);
            return clients;
        }


        public async Task<int?> AddClientAsync(NewClientDto newClientDto)
        {
            var newClient = new ClientWithAdvisor()
            {
                FirstName = newClientDto.FirstName,
                LastName = newClientDto.LastName,
                AdvisorId = newClientDto.AdvisorId,
                
            };

            var createdClientId = await _repository.AddAsync(newClient);
            return createdClientId;
        }

        public async Task<bool?> UpdateClientAsync(int id, ClientDto clientDto)
        {
            var clientToUpdate = await _repository.GetByIdAsync(id);
            if (clientToUpdate == null)
            {
                return null;
            }

            var updatedClient = new ClientWithAdvisor()
            {
                Id = id,
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                AdvisorId = clientDto.AdvisorId
            };

            var result = await _repository.UpdateAsync(updatedClient);
            return result;
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            var result = await _repository.DeleteAsync(id);
            return result;
        }
    }
}
