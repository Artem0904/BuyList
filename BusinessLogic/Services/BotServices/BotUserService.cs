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
        public readonly IImageService imageService;
        public readonly IMapper mapper;
        public BotUserService(IRepository<BotUser> botUserRepository,
            IImageService imageService,
            IMapper mapper)
        {
            this.botUserRepository = botUserRepository;
            this.mapper = mapper;
            this.imageService = imageService;
        }
        public async Task DeleteAsync(long chatId)
        {
            var botUser = await botUserRepository.GetItemBySpec(new BotUserSpecs.GetByChatId(chatId));
            if (botUser != null)
            {
                if (!String.IsNullOrEmpty(botUser.Image))
                {
                    imageService.DeleteImageIfExists(botUser.Image);
                }
                await botUserRepository.DeleteAsync(botUser.Id);
                await botUserRepository.SaveAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var botUser = await botUserRepository.GetItemBySpec(new BotUserSpecs.GetById(id));
            if (botUser != null)
            {
                if (!String.IsNullOrEmpty(botUser.Image))
                {
                   imageService.DeleteImageIfExists(botUser.Image);
                }
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
