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
        public readonly IRepository<BotUser> useRepository;

        public AccountService(IRepository<BotUser> useRepository)
        {
            this.useRepository = useRepository; 
        }
        public Task AddAsync(BaseBotUserModel createModel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(BotUser user)
        {
            throw new NotImplementedException();
        }
    }
}
