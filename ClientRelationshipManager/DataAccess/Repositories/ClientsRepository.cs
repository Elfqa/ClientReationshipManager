using BusinessLogic.Models;
using DataAccess.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.VisualBasic;

namespace DataAccess.Repositories
{
    public class ClientsRepository : IClientsRepository
    {
        private IDapperContext _context;

        public ClientsRepository(IDapperContext context)
        {
            _context = context;
        }
        
        public async Task<IEnumerable<ClientWithAdvisor>> GetAllAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = "select c.Id, c.FirstName, c.LastName, a.Id as AdvisorId, a.Name as AdvisorName " +
                          "from clients c " +
                          "left join ClientAdvisorRelationships ca on ca.ClientId=c.Id " +
                          "left join Advisors a on a.Id=ca.AdvisorId";

                var clients = await connection.QueryAsync<ClientWithAdvisor>(sql);  
                return clients;
            }
        }

        public async Task<ClientWithAdvisor> GetByIdAsync(int id)
        {
            var sql = "select c.Id, c.FirstName, c.LastName, a.Id as AdvisorId, a.Name as AdvisorName " +
                      "from clients c " +
                      "left join ClientAdvisorRelationships ca on ca.ClientId=c.Id " +
                      "left join Advisors a on a.Id=ca.AdvisorId " +
                      "WHERE c.Id = @Id";

            using (var connection = _context.CreateConnection())
            {
                var client = await connection.QuerySingleOrDefaultAsync<ClientWithAdvisor>(sql, new { Id = id });
                return client;
            }
        }

        public async Task<IEnumerable<ClientWithAdvisor>> GetAllByAdvisorIdAsync(int id)
        {
            var sql = "select c.Id, c.FirstName, c.LastName, a.Id as AdvisorId, a.Name as AdvisorName " +
                      "from clients c " +
                      "left join ClientAdvisorRelationships ca on ca.ClientId=c.Id " +
                      "left join Advisors a on a.Id=ca.AdvisorId " +
                      "WHERE ca.AdvisorId = @Id";

            using (var connection = _context.CreateConnection())
            {
                var contacts = await connection.QueryAsync<ClientWithAdvisor>(sql, new { Id = id });
                return contacts;
            }
        }

        public async Task<int> AddAsync(ClientWithAdvisor entity)
        {
            var sql = "INSERT INTO Clients (FirstName, LastName) VALUES (@FirstName, @LastName); " +
                      "SELECT CAST(SCOPE_IDENTITY() as int); " +
                      "INSERT INTO ClientAdvisorRelationships (AdvisorId, ClientId, LastUpdate) " +
                      "VALUES (@AdvisorId, (SELECT MAX(Id) FROM Clients), GETDATE()); ";

            using (var connection = _context.CreateConnection())
            {

                //an exception may appear when trying to add a relation to a non-existent advisor
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

        public async Task<bool> UpdateAsync(ClientWithAdvisor entity)
        {
           
            if (string.IsNullOrWhiteSpace(entity.FirstName) && string.IsNullOrWhiteSpace(entity.LastName) && entity.AdvisorId == null)
            {
                return false;
            }

            var sql = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(entity.FirstName))
            {
                sql.Append("UPDATE Clients SET FirstName = @FirstName WHERE Id = @Id; ");
            }
            if (!string.IsNullOrWhiteSpace(entity.LastName))
            {
                sql.Append("UPDATE Clients SET LastName = @LastName WHERE Id = @Id; ");
            }
            if (entity.AdvisorId != null )
            {
                sql.Append("UPDATE ClientAdvisorRelationships SET AdvisorId = @AdvisorId, LastUpdate=GETDATE() WHERE ClientId = @Id ");
            }

            using (var connection = _context.CreateConnection())
            {
                
                //an exception may appear when trying to update a contact to a non-existent advisor
                try
                {
                    var affectedRows = await connection.ExecuteAsync(sql.ToString(), entity);
                    return affectedRows > 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = "DELETE FROM ClientAdvisorRelationships WHERE ClientId = @Id " +
                                "DELETE FROM Clients WHERE Id = @Id";

                var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
                return affectedRows > 0;
            }
        }

        /* additional functions never used
         * getClients with Advisor model
         
        private Client Map(Client client, UserAdvisor advisor)
           {
               if(advisor != null)
                   client.Advisor = advisor;
               return client;
           }

        public async Task<IEnumerable<Client>> GetAllAsync()
           {
               using (var connection = _context.CreateConnection())
               {
                   var sql = "select c.Id, c.FirstName, c.LastName, a.Id, a.Name, a.Email " +
                             "from clients c " +
                             "left join ClientAdvisorRelationships ca on ca.ClientId=c.Id " +
                             "left join Advisors a on a.Id=ca.AdvisorId";

                   var clients = await connection.QueryAsync<Client, UserAdvisor, Client>(sql, Map);    //splitOn: Id from Advisors
                   return clients;
               }
           }


           public async Task<IEnumerable<Client>> GetByIdAsync(int id)
           {
               using (var connection = _context.CreateConnection())
               {
                   var sql = "select c.Id, c.FirstName, c.LastName, a.Id, a.Name, a.Email " +
                             "from clients c " +
                             "left join ClientAdvisorRelationships ca on ca.ClientId=c.Id " +
                             "left join Advisors a on a.Id=ca.AdvisorId " +
                             "WHERE c.Id = @Id";

                   var clients = await connection.QueryAsync<Client, UserAdvisor, Client>(sql, Map, new { Id = id });    //splitOn: Id from Advisors
                   return clients;
               }
           }
         */
    }
}
