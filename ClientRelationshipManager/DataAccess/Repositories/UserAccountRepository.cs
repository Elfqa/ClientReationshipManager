using BusinessLogic.Models;
using Dapper;
using DataAccess.DAL;

namespace DataAccess.Repositories;

public class UserAccountRepository : IUserAccountRepository
{
    private IDapperContext _context;

    public UserAccountRepository(IDapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserAccount>> GetAllAsync()
    {
        using (var connection = _context.CreateConnection())
        {
            var users = await connection.QueryAsync<UserAccount>("SELECT * FROM Advisors");
            return users;
        }
    }

    public async Task<UserAccount?> Authenticate(string name, string password)
    {
        using (var connection = _context.CreateConnection())
        {
            var sql = "SELECT Id, Name, Email FROM Advisors WHERE Name = @Name AND Password = @Password";
            var user = await connection.QuerySingleOrDefaultAsync<UserAccount>(sql, new { name, password });
            return user;
        }
    }
}