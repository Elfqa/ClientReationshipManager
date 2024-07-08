using BusinessLogic.Models;
using Dapper;
using DataAccess.DAL;

namespace DataAccess.Repositories;

public class UserAccountRepository 
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

}