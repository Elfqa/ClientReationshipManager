using BusinessLogic.Models;
using DataAccess.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DataAccess.Repositories
{
    public class ContactsRepository : IRepository<Contact>
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
                var contacts = await connection.QueryAsync<Contact>("SELECT * FROM Contacts");
                return contacts;
            }
        }

        public async Task<Contact> GetByIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var contacts = await connection.QuerySingleOrDefaultAsync<Contact>("SELECT * FROM Contacts WHERE Id = @Id", new { Id = id });
                return contacts;
            }
        }

        public Task<IEnumerable<Contact>> GetByIdAsync2(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Contact>> GetAllByAdvisorIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var contacts = await connection.QueryAsync<Contact>("SELECT * FROM Contacts WHERE AdvisorId = @Id", new { Id = id });
                return contacts;
            }
        }
        public async Task<IEnumerable<Contact>> GetAllByClientIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var contacts = await connection.QueryAsync<Contact>("SELECT * FROM Contacts WHERE ClientId = @Id", new { Id = id });
                return contacts;
            }
        }




        //--------------------------
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
                    throw new Exception("Błąd dodania nowego kontaktu");
                }

            }
        }

        // public string Description { get; set; }

        // public DateTime LastUpdate { get; set; }
        // public DateTime? ScheduledDate { get; set; }
        // public DateTime? StartDate { get; set; }
        // public DateTime? EndDate { get; set; }
        // public ContactStatus Status { get; set; }


        public async Task<bool> UpdateAsync(Contact entity)
        {
            //without the AdvisorId and ClinetID parameters because they are not editable

            var sql = new StringBuilder("UPDATE Contacts SET LastUpdate = @LastUpdate, Status = @Status ");

            //var parameters = new DynamicParameters();
            // parameters.Add("@Id", entity.Id);;


            if (!string.IsNullOrWhiteSpace(entity.Description))
            {
                sql.Append(", Description = @Description ");
                //parameters.Add("@Description", entity.Description);
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

            
            using (var connection = _context.CreateConnection())
            {
                var affectedRows = await connection.ExecuteAsync(sql.ToString(), entity);//do parametrów podstawiamy caly obiekt entity nawet jesli ma puste niektore wlasciwosci
                return affectedRows > 0;
            }
        }


        //public async Task<bool> UpdateAsync(Contact entity)
        //{
        //    var sql = "UPDATE Contacts SET Description = @Description, LastUpdate = @LastUpdate, " +
        //              "StartDate = @StartDate, EndDate = @EndDate, Status = @Status " +
        //              "WHERE Id = @Id";

        //    using (var connection = _context.CreateConnection())
        //    {
        //        var affectedRows = await connection.ExecuteAsync(sql, entity);
        //        return affectedRows > 0;
        //    }
        //}



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
