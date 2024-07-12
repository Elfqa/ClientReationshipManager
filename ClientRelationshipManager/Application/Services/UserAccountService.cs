using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Models;

namespace Application.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _repository;

        public UserAccountService(IUserAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserAccount?> Authenticate(string name, string password)
        {
            return await _repository.Authenticate(name, password);
        }
    }
}
