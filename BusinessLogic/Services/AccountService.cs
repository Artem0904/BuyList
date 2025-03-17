using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;
using BusinessLogic.Models.UserModels;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        public readonly IRepository<User> useRepository;

        public AccountService(IRepository<User> useRepository)
        {
            this.useRepository = useRepository; 
        }
        public Task AddAsync(BaseUserModel createModel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
