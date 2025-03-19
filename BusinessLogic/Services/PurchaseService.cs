using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;
using BusinessLogic.Models.PurchaseModels;
using BusinessLogic.Specifications;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class PurchaseService : IPurchaseService
    {
        public readonly IRepository<Purchase> orderRepository;
        public readonly IMapper mapper;

        public PurchaseService(IRepository<Purchase> orderRepository, 
            IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<PurchaseDto>> GetAllAsync()
        {
            return mapper.Map<IEnumerable<PurchaseDto>>(await orderRepository.GetListBySpec(new OrderSpecs.GetAll()));
        }

        public async Task<PurchaseDto> GetByIdAsync(int id)
        {
            return mapper.Map<PurchaseDto>(await orderRepository.GetItemBySpec(new OrderSpecs.GetById(id)));
        }

        public async Task<PurchaseDto> CreateAsync(BasePurchaseModel creationModel, int userId)
        {
            var order = mapper.Map<Purchase>(creationModel);
            order.UserId = userId;

            await orderRepository.InsertAsync(order);
            await orderRepository.SaveAsync();
            return mapper.Map<PurchaseDto>(order);
        }

        public async Task DeleteAsync(int id)
        {
            var order = mapper.Map<PurchaseDto>(await orderRepository.GetItemBySpec(new OrderSpecs.GetById(id)));
            if (order != null) {
                await orderRepository.DeleteAsync(id);
                await orderRepository.SaveAsync();
            }
        }
    }
}
