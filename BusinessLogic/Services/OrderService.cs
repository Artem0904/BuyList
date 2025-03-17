using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic.DTOs;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;
using BusinessLogic.Models.OrderModels;
using BusinessLogic.Specifications;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class OrderService : IOrderService
    {
        public readonly IRepository<Order> orderRepository;
        public readonly IMapper mapper;

        public OrderService(IRepository<Order> orderRepository, 
            IMapper mapper)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            return mapper.Map<IEnumerable<OrderDto>>(await orderRepository.GetListBySpec(new OrderSpecs.GetAll()));
        }

        public async Task<OrderDto> GetByIdAsync(int id)
        {
            return mapper.Map<OrderDto>(await orderRepository.GetItemBySpec(new OrderSpecs.GetById(id)));
        }

        public async Task<OrderDto> CreateAsync(BaseOrderModel creationModel, int userId)
        {
            var order = mapper.Map<Order>(creationModel);
            order.UserId = userId;

            await orderRepository.InsertAsync(order);
            await orderRepository.SaveAsync();
            return mapper.Map<OrderDto>(order);
        }

        public async Task DeleteAsync(int id)
        {
            var order = mapper.Map<OrderDto>(await orderRepository.GetItemBySpec(new OrderSpecs.GetById(id)));
            if (order != null) {
                await orderRepository.DeleteAsync(id);
                await orderRepository.SaveAsync();
            }
        }
    }
}
