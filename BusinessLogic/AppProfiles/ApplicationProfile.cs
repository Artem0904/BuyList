using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Entities;
using BusinessLogic.Models.OrderModels;

namespace BusinessLogic.AppProfiles
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<Order, OrderDto>()
                .ReverseMap();
            CreateMap<Order, BaseOrderModel>()
                .ReverseMap();

            CreateMap<BotUser, BotUserDto>()
                .ReverseMap();
        }
    }
}
