using BusinessLogic.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.DTOs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly IContactsService _contactsService;
        private readonly ILogger<ContactsController> _logger;

        
        public ContactsController(IContactsService contactsService, ILogger<ContactsController> logger)
        {
            _contactsService = contactsService;
            _logger = logger;
        }

        // GET: api/<ContactsController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contacts = await _contactsService.GetAllContacts();
            _logger.LogInformation($"Pobrano listę kontaktów.");
            return Ok(contacts);
        }


        
        // GET api/<ContactsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contact = await _contactsService.GetContactById(id);
            if (contact == null)
            {
                _logger.LogInformation($"Nie znaleziono kontaktu o id {id}");
                return NotFound();
            }
            _logger.LogInformation($"Pobrano kontakt o id {id}.");
            return Ok(contact);
        }


        // POST api/<ContactsController>
        [HttpPost("schedule-a-contact")]
        public async Task<IActionResult> Add([FromBody] ContactToScheduleDto contactDto)
        {

            var createdContactId = await _contactsService.ScheduleContact(contactDto);
            
            if (createdContactId == null )
            {
                _logger.LogError("Błąd dodania nowego kontaktu. Niepoprawna data");
                return BadRequest();
                
            }
            if (createdContactId == 0)
            {
                _logger.LogError("Błąd dodania nowego kontaktu. Doradca lub klient nie istnieje");
                return BadRequest();

            }
            _logger.LogInformation($"Kontakt {contactDto.Description} został zaplanowany pod id {createdContactId}.");
            //return CreatedAtAction(nameof(GetContactById), new { id = createdContactId }, contactDto);//w resopne wyswietla nam utworzony objekt DTO i ścieżka z nowym id
            return Ok(createdContactId);
        }


        [HttpPut("edit-scheduled/{id}")]
        public async Task<IActionResult> UpdateScheduled(int id, [FromBody] ContactScheduledToUpdateDto contactDto)
        {
            var result = await _contactsService.UpdateScheduledContact(id, contactDto);
            
            if (result == null)
            {
                _logger.LogWarning($"Nie znaleziono kontaktu o id {id}");
                return NotFound();
            }

            if (result == false)
            {
                _logger.LogWarning($"Nie można przesunąć kontaktu");
                return BadRequest();
            }
            _logger.LogInformation($"Kontakt o id {id} został zaktualizowany.");
            return NoContent();
        }

        // PUT api/<ContactsController>/5
        [HttpPut("mark-as-completed/{id}")]
        public async Task<IActionResult> UpdateScheduledToCompleted(int id, [FromBody] ContactCompletedDto contactDto)
        {
            var result = await _contactsService.UpdateScheduledContactToCompleted(id, contactDto);

            if(result == null)
            {
                _logger.LogWarning($"Nie znaleziono kontaktu o id {id}");
                return NotFound();
            }

            if (result == false)
            {
                _logger.LogWarning($"Nie można oznaczyć kontaktu jako zrealizowany");
                return BadRequest();
            }
            _logger.LogInformation($"Kontakt o id {id} został zaktualizowany.");
            return NoContent();
        }


        // DELETE api/<ContactsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _contactsService.DeleteContact(id);
            if (!result)
            {
                _logger.LogInformation($"Nie znaleziono kontaktu o id {id}");
                return NotFound();
            }
            _logger.LogInformation($"Kontakt o id {id} został usunięty");
            return Ok(result);
        }

        

        [HttpGet("AdvisorId")]
        public async Task<IActionResult> GetAllByAdvisorIdAsync(int id)
        {
            var contact = await _contactsService.GetAllContactsByAdvisorId(id);
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
            var contacts = await _contactsService.GetAllContactsByClientId(id);
            if (contacts == null)
            {
                _logger.LogInformation($"Nie znaleziono kontaktu dla klienta o id {id}");
                return NotFound();
            }
            _logger.LogInformation($"Pobrano kontakty.");
            return Ok(contacts);
        }




        




    }

  
}
