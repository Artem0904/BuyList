using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Entities;
using BusinessLogic.Exceptions;
using BusinessLogic.Interfaces;
using BusinessLogic.Models.BalanceModels;
using BusinessLogic.Specifications;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.Services
{
    public class BalanceService : IBalanceService
    {
        public readonly IRepository<Balance> balanceRepository;
        public readonly IRepository<BotUser> botUserRepository;
        public readonly IMapper mapper;
        public BalanceService(IRepository<Balance> balanceRepository,
            IRepository<BotUser> botUserRepository,
            IMapper mapper)
        {
            this.balanceRepository = balanceRepository;
            this.botUserRepository = botUserRepository;
            this.mapper = mapper;
        }
        public async Task<BalanceDto> CreateAsync(BaseBalanceModel creationModel)
        {
            var user = await botUserRepository.GetItemBySpec(new BotUserSpecs.GetById(creationModel.UserId));
            if (user != null)
            {
                if(user.Balance == null)
                {
                    var balance = mapper.Map<Balance>(creationModel);
                    await balanceRepository.InsertAsync(balance);
                    await balanceRepository.SaveAsync();
                    return mapper.Map<BalanceDto>(balance); 
                }
                else
                {
                    var balance = await balanceRepository.GetItemBySpec(new BalanceSpecs.GetById(user.Balance.Id));
                    mapper.Map(creationModel, balance);
                    await balanceRepository.SaveAsync();
                    return mapper.Map<BalanceDto>(balance);
                }
            }
            throw new HttpException("Bad", HttpStatusCode.BadRequest);
        }

        public async Task DeleteAsync(int id)
        {
            var balance = mapper.Map<Balance>(await balanceRepository.GetItemBySpec(new BalanceSpecs.GetById(id)));
            if (balance != null)
            {
                await balanceRepository.DeleteAsync(balance.Id);
                await balanceRepository.SaveAsync();

            }
        }

        public async Task<IEnumerable<BalanceDto>> GetAllAsync()
        {
            return mapper.Map<IEnumerable<BalanceDto>>(await balanceRepository.GetListBySpec(new BalanceSpecs.GetAll()));
        }

        public async Task<BalanceDto> GetByIdAsync(int id)
        {
            return mapper.Map<BalanceDto>(await balanceRepository.GetItemBySpec(new BalanceSpecs.GetById(id)));

        }
    }
}
