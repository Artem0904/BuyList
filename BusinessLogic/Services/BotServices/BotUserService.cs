using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;
using BusinessLogic.Specifications;
using DataAccess.Repositories;
using Telegram.Bot.Types;

namespace BusinessLogic.Services.BotServices
{
    public class BotUserService : IBotUserService
    {
        public readonly IRepository<BotUser> botUserRepository;
        public readonly IMapper mapper;
        public BotUserService(IRepository<BotUser> botUserRepository, 
            IMapper mapper)
        {
            this.botUserRepository = botUserRepository;
            this.mapper = mapper;
        }
        public async Task DeleteAsync(long chatId)
        {
            var botUser = await botUserRepository.GetItemBySpec(new BotUserSpecs.GetByChatId(chatId));
            if (botUser != null)
            {
                await botUserRepository.DeleteAsync(botUser.Id);
                await botUserRepository.SaveAsync();
            }
        }

        public async Task<IEnumerable<BotUserDto>> GetAllAsync()
        {
            return mapper.Map<IEnumerable<BotUserDto>>(await botUserRepository.GetListBySpec(new BotUserSpecs.GetAll()));
        }

        public async Task<BotUserDto> GetByChatIdAsync(long chatId)
        {
            return mapper.Map<BotUserDto>(await botUserRepository.GetItemBySpec(new BotUserSpecs.GetByChatId(chatId)));
        }

        public async Task<BotUserDto> GetByIdAsync(int userId)
        {
            return mapper.Map<BotUserDto>(await botUserRepository.GetItemBySpec(new BotUserSpecs.GetById(userId)));
        }
    }
}
