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
    public class ClientsRepository : IClientsRepository
    {
        private IDapperContext _context;

        public ClientsRepository(IDapperContext context)
        {
            _context = context;
        }
        private Client Map(Client client, UserAdvisor advisor)
        {
            if(advisor != null)
                client.Advisor = advisor;
            return client;
        }

        public async Task<IEnumerable<ClientWithAdvisor>> GetAllAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = "select c.Id, c.FirstName, c.LastName, a.Id as AdvisorId, a.Name as AdvisorName " +
                          "from clients c " +
                          "left join ClientAdvisorRelationships ca on ca.ClientId=c.Id " +
                          "left join Advisors a on a.Id=ca.AdvisorId";

                var clients = await connection.QueryAsync<ClientWithAdvisor>(sql);    //splitOn: Id from Advisors
                return clients;
            }
        }

        //public async Task<IEnumerable<Client>> GetAllAsync()
        //{
        //    using (var connection = _context.CreateConnection())
        //    {
        //        var sql = "select c.Id, c.FirstName, c.LastName, a.Id, a.Name, a.Email " +
        //                  "from clients c " +
        //                  "left join ClientAdvisorRelationships ca on ca.ClientId=c.Id " +
        //                  "left join Advisors a on a.Id=ca.AdvisorId";

        //        var clients = await connection.QueryAsync<Client,UserAdvisor, Client>(sql, Map);    //splitOn: Id from Advisors
        //        return clients;
        //    }
        //}

        public Task<Client> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

      

        public async Task<IEnumerable<Client>> GetByIdAsync2(int id)
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


        public async Task<int> AddAsync(Client entity)
        {
            var sql = "INSERT INTO Clients (FirstName, LastName) VALUES (@FirstName, @LastName); " +
                      "SELECT CAST(SCOPE_IDENTITY() as int); " +
                      "INSERT INTO ClientAdvisorRelationships (AdvisorId, ClientId, LastUpdate) " +
                      "VALUES (5, (SELECT MAX(Id) FROM Clients), GETDATE()); ";

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
                    throw new Exception("Błąd dodania nowego klienta");
                }

            }
        }


        public async Task<IEnumerable<Client>> GetAllByAdvisorIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var contacts = await connection.QueryAsync<Client>("SELECT * FROM Contacts WHERE AdvisorId = @Id", new { Id = id });
                return contacts;
            }
        }
        



        public Task<bool> UpdateAsync(Client entity)
        {
            throw new NotImplementedException();
        }


        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var affectedRows = await connection.ExecuteAsync("DELETE FROM Clients WHERE Id = @Id", new { Id = id });
                return affectedRows > 0;
            }
        }
    }
}
