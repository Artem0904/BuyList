﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.Entities;
using BusinessLogic.Enums.Roles;
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
        private readonly IImageService imageService;
        private readonly UserManager<BotUser> userManager;
        public AccountService(IRepository<BotUser> useRepository, 
            IMapper mapper,
            IImageService imageService,
            UserManager<BotUser> userManager)
        {
            this.useRepository = useRepository;
            this.mapper = mapper;
            this.userManager = userManager;
            this.imageService = imageService;
        }
        public async Task AddAsync(BaseBotUserModel createModel)
        {
            var user = mapper.Map<BotUser>(createModel);
            if (user.Id == 0)
            {
                if (createModel.ImageUrl != null)
                {
                    user.Image = await imageService.SaveImageFromUrlAsync(createModel.ImageUrl);
                }
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
