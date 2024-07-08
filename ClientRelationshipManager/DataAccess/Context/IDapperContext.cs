using System.Data;

namespace DataAccess.DAL
{
    public interface IDapperContext
    {
        //interfejs dodalismy dopiero przy wdrazaniu tesów - ogolnie mogloby byc bez interfejsu
        public IDbConnection CreateConnection();
    }
}
