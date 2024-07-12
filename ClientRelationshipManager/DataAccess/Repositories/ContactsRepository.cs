using BusinessLogic.Models;
using DataAccess.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DataAccess.Repositories
{
    public class ContactsRepository : IContactsRepository
    {
        private IDapperContext _context;

        public ContactsRepository(IDapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                var contacts = await connection.QueryAsync<Contact>("SELECT * FROM Contacts ORDER BY Id DESC");
                return contacts;
            }
        }

        public async Task<Contact> GetByIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var contacts = await connection.QuerySingleOrDefaultAsync<Contact>("SELECT * FROM Contacts WHERE Id = @Id ORDER BY Id DESC", new { Id = id });
                return contacts;
            }
        }

        public async Task<IEnumerable<Contact>> GetAllByAdvisorIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var contacts = await connection.QueryAsync<Contact>("SELECT * FROM Contacts WHERE AdvisorId = @Id ORDER BY Id DESC", new { Id = id });
                return contacts;
            }
        }
        public async Task<IEnumerable<Contact>> GetAllByClientIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var contacts = await connection.QueryAsync<Contact>("SELECT * FROM Contacts WHERE ClientId = @Id ORDER BY Id DESC", new { Id = id });
                return contacts;
            }
        }

        public async Task<int> AddAsync(Contact entity)
        {
            var sql = "INSERT INTO Contacts (Description, CreationDate, LastUpdate, ScheduledDate, StartDate, EndDate, Status, AdvisorId, ClientId) " +
                            "VALUES (@Description, @CreationDate, @LastUpdate, @ScheduledDate, @StartDate, @EndDate, @Status, @AdvisorId, @ClientId); " +
                            "SELECT CAST(SCOPE_IDENTITY() as int)";
            
            using (var connection = _context.CreateConnection())
            {

                //an exception may appear when trying to add a contact to a non-existent client or advisor
                try
                {
                    var id = await connection.QuerySingleAsync<int>(sql, entity);
                    return id;
                }
                catch (Exception)                  
                {
                    return 0;
                }

            }
        }

        public async Task<bool> UpdateAsync(Contact entity)
        {
            //Update func without the AdvisorId and ClinetID parameters because they are not editable

            var sql = new StringBuilder("UPDATE Contacts SET LastUpdate = @LastUpdate, Status = @Status ");

            //Description,ScheduledDate,StartDate,EndDate will not be edited if they are passed empty from the model
            if (!string.IsNullOrWhiteSpace(entity.Description))
            {
                sql.Append(", Description = @Description ");
            }
            if (entity.ScheduledDate != null)
            {
                sql.Append(", ScheduledDate = @ScheduledDate ");
            }
            if (entity.StartDate != null)
            {
                sql.Append(", StartDate = @StartDate ");
            }
            if (entity.EndDate != null)
            {
                sql.Append(", EndDate = @EndDate ");
            }

            sql.Append("WHERE Id = @Id");

            //I put the entire entity object into the parameters, even if it has some empty properties, because it is secured in the SQL UPDATE clause
            using (var connection = _context.CreateConnection())
            {
                var affectedRows = await connection.ExecuteAsync(sql.ToString(), entity);
                return affectedRows > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var affectedRows = await connection.ExecuteAsync("DELETE FROM Contacts WHERE Id = @Id", new { Id = id });
                return affectedRows > 0;
            }
        }
    }
}
