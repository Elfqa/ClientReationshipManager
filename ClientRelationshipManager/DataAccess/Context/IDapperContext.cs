using System.Data;

namespace DataAccess.DAL
{
    public interface IDapperContext
    {
        public IDbConnection CreateConnection();
    }
}
