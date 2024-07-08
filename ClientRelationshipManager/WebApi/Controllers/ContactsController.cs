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
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly IRepository<Contact> _repository;
        //private readonly ContactsRepository _repository;            //mozna tez zmienic wstrzykniecie na zwykle repo a nie interfejs, bo wtedy w IRepository i innych kontrolerach musza tez byc te metody 
        private readonly ILogger<ContactsController> _logger;

        
        public ContactsController(IRepository<Contact> repository, ILogger<ContactsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET: api/<ContactsController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contacts = await _repository.GetAllAsync();
            _logger.LogInformation($"Pobrano listę kontaktów.");
            return Ok(contacts);
        }



        // GET api/<ContactsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contact = await _repository.GetByIdAsync(id);
            if (contact == null)
            {
                _logger.LogInformation($"Nie znaleziono kontaktu o id {id}");
                return NotFound();
            }
            _logger.LogInformation($"Pobrano kontakt o id {id}.");
            return Ok(contact);
        }


        [HttpGet("AdvisorId")]
        public async Task<IActionResult> GetAllByAdvisorIdAsync(int id)
        {
            var contact = await _repository.GetAllByAdvisorIdAsync(id);
            if (contact == null)
            {
                _logger.LogInformation($"Nie znaleziono kontaktu dla doradcy o id {id}");
                return NotFound();
            }
            _logger.LogInformation($"Pobrano kontakty.");
            return Ok(contact);
        }

        [HttpGet("ClientId")]
        public async Task<IActionResult> GetAllByClientIdAsync(int id)
        {
            var contacts = await _repository.GetAllByClientIdAsync(id);
            if (contacts == null)
            {
                _logger.LogInformation($"Nie znaleziono kontaktu dla klienta o id {id}");
                return NotFound();
            }
            _logger.LogInformation($"Pobrano kontakty.");
            return Ok(contacts);
        }





        // POST api/<ContactsController>
        [HttpPost ("schedule-a-contact")]
        public async Task<IActionResult> Add([FromBody] ContactScheduledDto contactDto)
        {

            var newContact = new Contact()    
            { 
                Description = contactDto.Description,
                CreationDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                ScheduledDate = contactDto.ScheduledDate,
                Status = ContactStatus.Scheduled,
                AdvisorId = contactDto.AdvisorId,
                ClientId = contactDto.ClientId,
            };


            var createdContactId = await _repository.AddAsync(newContact);
            _logger.LogInformation($"Kontakt {newContact.Description} został dodany do bazy danych.");

            //return CreatedAtAction(nameof(GetById), new { id = createdContactId }, contactDto);//w resopne wyswietla nam utworzony objekt DTO i ścieżka z nowym id
            return Ok(createdContactId); 
        }






        // PUT api/<ContactsController>/5
        [HttpPut("mark-as-completed/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ContactCompletedDto contactDto)
        {
            var contactToEdit = await _repository.GetByIdAsync(id);
            if (contactToEdit == null)
            {
                _logger.LogInformation($"Nie znaleziono kontaktu o id {id}");
                return NotFound();
            }

            if (contactToEdit.Status == ContactStatus.Completed)
            {
                _logger.LogInformation($"Nie można edytować zrealizowanego kontaktu");
                return BadRequest();
            }

            var contact = new Contact()    
            {
                Id = id,
                Description = contactDto.Description,
                LastUpdate = DateTime.Now,
                StartDate = contactDto.StartDate,
                EndDate = contactDto.EndDate,
                Status = ContactStatus.Completed,
            };

            var result = await _repository.UpdateAsync(contact);
            //if (!result)
            //{
            //    _logger.LogInformation($"Nie znaleziono kontaktu o id {id}");
            //    return NotFound();
            //}
            _logger.LogInformation($"Kontakt o id {id} został zaaktualizowany.");
            return NoContent(); 
        }


        [HttpPut("edit-scheduled/{id}")] public async Task<IActionResult> UpdateScheduledAsync(int id, [FromBody] ContactScheduledDto contactDto)
        {

            var contactToEdit = await _repository.GetByIdAsync(id);
            if (contactToEdit == null)
            {
                _logger.LogInformation($"Nie znaleziono kontaktu o id {id}");
                return NotFound();
            }

            if (contactToEdit.Status == ContactStatus.Completed)
            {
                _logger.LogInformation($"Nie można przesunąc zrealizowanego kontaktu");
                return BadRequest();
            }


            var contact = new Contact()
            {
                Id = id,
                Description = contactDto.Description,
                LastUpdate = DateTime.Now,
                ScheduledDate = contactDto.ScheduledDate,
                Status = ContactStatus.Scheduled,
                //does not pass the AdvisorId and ClinetID parameters because they are not editable
            };

            var result = await _repository.UpdateAsync(contact);
            //if (!result)
            //{
            //    _logger.LogInformation($"Nie znaleziono kontaktu o id {id}");
            //    return NotFound();
            //}
            _logger.LogInformation($"Kontakt o id {id} został zaaktualizowany.");
            return NoContent();
        }





        // DELETE api/<ContactsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
            {
                _logger.LogInformation($"Nie znaleziono kontaktu o id {id}");
                return NotFound();
            }
            _logger.LogInformation($"Kontakt o id {id} został usunięty");
            return NoContent();
            //return Ok(result);//jesli chce przekazac true/false
        }
    }
}
