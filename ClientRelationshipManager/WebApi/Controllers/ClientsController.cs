using BusinessLogic.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsRepository _repository;
        private readonly ILogger<ClientsController> _logger;


        public ClientsController(IClientsRepository repository, ILogger<ClientsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET: api/<ClientsController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _repository.GetAllAsync();
            _logger.LogInformation($"Pobrano listę klientów.");
            return Ok(clients);
        }


        // GET: api/<ClientsController>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _repository.GetByIdAsync2(id);
            if (client == null)
            {
                _logger.LogInformation($"Nie znaleziono klienta o id {id}");
                return NotFound();
            }
            _logger.LogInformation($"Pobrano klienta o id {id}.");
            return Ok(client);
        }



        // POST api/<ClientsController>
        [HttpPost]
       public async Task<IActionResult> Add([FromBody] ClientDto clientDto)
        {

            var newClient = new Client()
            {
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                
            };
            
            var createdClientId = await _repository.AddAsync(newClient);
            _logger.LogInformation($"Klient został dodany do bazy danych.");

            return Ok(createdClientId);
        }


        // PUT api/<ClientsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ClientsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


    }
}
