using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetBot;
using AutoMapper;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;
using BusinessLogic.Models.UserModels;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        public readonly IRepository<BotUser> useRepository;
        private readonly IMapper mapper;
        private readonly UserManager<BotUser> userManager;
        public AccountService(IRepository<BotUser> useRepository, 
            IMapper mapper, 
            UserManager<BotUser> userManager)
        {
            this.useRepository = useRepository;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        public async Task AddAsync(BaseBotUserModel createModel)
        {
            var user = mapper.Map<BotUser>(createModel);
            if (user.Id == 0)
            {
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, Roles.User.ToString());
            }
            else
            {
                await userManager.UpdateAsync(user);
            }
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
