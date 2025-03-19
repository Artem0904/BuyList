using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Entities;
using BusinessLogic.Models.BalanceModels;
using BusinessLogic.Models.PurchaseModels;
using BusinessLogic.Models.UserModels;

namespace BusinessLogic.AppProfiles
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Purchase, PurchaseDto>()
                .ReverseMap();
            CreateMap<Purchase, BasePurchaseModel>()
                .ReverseMap();

            CreateMap<BotUser, BotUserDto>()
                .ForMember(x => x.Balance, opt => opt.MapFrom(y => y.Balance != null ? y.Balance.Money : 0))
                .ReverseMap();

            CreateMap<BotUser, BaseBotUserModel>()
               .ReverseMap();


            CreateMap<Balance, BalanceDto>()
               .ReverseMap();

            CreateMap<Balance, BaseBalanceModel>()
               .ReverseMap();

        }
    }
}
