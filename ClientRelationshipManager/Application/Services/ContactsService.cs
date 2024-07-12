using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Models;
using DataAccess.Repositories;
using Application.DTOs;



namespace Application.Services
{
    public class ContactsService : IContactsService
    {
        private readonly IContactsRepository _repository;
        

        public ContactsService(IContactsRepository repository)
        {
            _repository = repository;
            
        }


        public async Task<IEnumerable<Contact>?> GetAllContacts()
        {
            var contacts = await _repository.GetAllAsync();
            return contacts;
        }

        
        public async Task<Contact?> GetContactById(int id)
        {
            var contact = await _repository.GetByIdAsync(id);
            return contact;
        }


        public async Task<int?> ScheduleContact(NewContactToScheduleDto contactDto)
        {
            if (contactDto.ScheduledDate < DateTime.Now || contactDto.ScheduledDate == null)
            {
                return null;
            }
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
           return createdContactId;
        }



        public async Task<bool?> UpdateScheduledContact(int id, ContactScheduledToUpdateDto contactDto)
        {

            var contactToEdit = await _repository.GetByIdAsync(id);
            if (contactToEdit == null || contactToEdit.Status == ContactStatus.Completed)
            {
                return null;
            }

            if (contactDto.ScheduledDate < DateTime.Now || contactDto.ScheduledDate == null)
            {
                return false;
            }
            
            var contact = new Contact()
            {
                Id = id,
                Description = contactDto.Description,
                LastUpdate = DateTime.Now,
                ScheduledDate = contactDto.ScheduledDate,
                Status = ContactStatus.Scheduled,
                //do not pass the AdvisorId and ClinetID parameters because they are not editable
            };

            var result = await _repository.UpdateAsync(contact);
            return result;
        }

        public async Task<bool?> UpdateScheduledContactToCompleted(int id, ContactCompletedDto contactDto)
        {
            var contactToEdit = await _repository.GetByIdAsync(id);
            if (contactToEdit == null)
            {
                return null;
            }
            if (contactDto.StartDate >= contactDto.EndDate || contactDto.EndDate >= DateTime.Now)//allow to edit both: completed and scheduled contact
            {
                return false;
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
            return result;
        }

       
        public async Task<bool> DeleteContact(int id)
        {
            var result = await _repository.DeleteAsync(id);
            return result;
        }

       


        public async Task<IEnumerable<Contact>?> GetAllContactsByAdvisorId(int id)
        {
            var contacts = await _repository.GetAllByAdvisorIdAsync(id);
            return contacts;
        }

        
        public async Task<IEnumerable<Contact>?> GetAllContactsByClientId(int id)
        {
            var contacts = await _repository.GetAllByClientIdAsync(id);
            return contacts;
        }

       
    }
}
