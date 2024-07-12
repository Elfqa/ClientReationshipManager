using Application.DTOs;
using Application.Services;
using BusinessLogic.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsService _clientsService;
        private readonly ILogger<ClientsController> _logger;

        public ClientsController(IClientsService clientsService, ILogger<ClientsController> logger)
        {
            _clientsService = clientsService;
            _logger = logger;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var clients = await _clientsService.GetAllClientsAsync();
            _logger.LogInformation("Pobrano listę klientów.");
            return Ok(clients);
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var client = await _clientsService.GetClientByIdAsync(id);
            if (client == null)
            {
                _logger.LogInformation($"Nie znaleziono klienta o id {id}");
                //return NotFound(new List<ClientWithAdvisor>());
                return NotFound();
            }
            _logger.LogInformation($"Pobrano klienta o id {id}.");
            //return Ok(new List<ClientWithAdvisor> { client });
            return Ok(client);
        }


        [HttpGet("AdvisorId")]
        public async Task<IActionResult> GetAllByAdvisorIdAsync(int id)
        {
            var clients = await _clientsService.GetAllClientsByAdvisorIdAsync(id);
            if (clients == null)
            {
                _logger.LogInformation($"Nie znaleziono klientów dla doradcy o id {id}");
                return NotFound();
            }
            _logger.LogInformation($"Pobrano klientów.");
            return Ok(clients);
        }

        // POST api/<ClientsController>
        [HttpPost]
       public async Task<IActionResult> Add([FromBody] NewClientDto newClientDto)
        {
            var createdClientId = await _clientsService.AddClientAsync(newClientDto);

            if (createdClientId == 0)
            {
                _logger.LogError("Błąd dodania nowego klienta.");
                return BadRequest();
            }
           
            _logger.LogInformation($"Klient {newClientDto.FirstName} {newClientDto.LastName} został dodany pod id {createdClientId}.");
            return Ok(createdClientId);
        }



        // PUT: api/Clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] ClientDto clientDto)
        {
            var result = await _clientsService.UpdateClientAsync(id, clientDto);

            if (result == null)
            {
                _logger.LogWarning($"Nie znaleziono klienta o id {id}");
                return NotFound();
            }

            if (result == false)
            {
                _logger.LogWarning("Błąd aktualizacji klienta.");
                return BadRequest();
            }

            _logger.LogInformation($"Klient o id {id} został zaktualizowany.");
            return NoContent();
        }


        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await _clientsService.DeleteClientAsync(id);
            if (!result)
            {
                _logger.LogInformation($"Nie znaleziono klienta o id {id}");
                return NotFound();
            }

            _logger.LogInformation($"Klient o id {id} został usunięty.");
            return Ok(result);
        }

    }
}
