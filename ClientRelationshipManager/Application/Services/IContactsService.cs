using Application.DTOs;
using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IContactsService
    {
        Task<IEnumerable<Contact>?> GetAllContacts();
        Task<Contact?> GetContactById(int id);
        Task<int?> ScheduleContact(NewContactToScheduleDto contactDto);
        Task<bool?> UpdateScheduledContact(int id, ContactScheduledToUpdateDto contactDto);
        Task<bool?> UpdateScheduledContactToCompleted(int id, ContactCompletedDto contactDto);
        Task<bool> DeleteContact(int id);
        Task<IEnumerable<Contact>?> GetAllContactsByAdvisorId(int id);
        Task<IEnumerable<Contact>?> GetAllContactsByClientId(int id);

    }
}
