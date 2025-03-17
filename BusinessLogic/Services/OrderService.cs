using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DTOs;
using BusinessLogic.Entities;
using BusinessLogic.Interfaces;
using BusinessLogic.Models.OrderModels;
using DataAccess.Repositories;

namespace BusinessLogic.Services
{
    public class OrderService : IOrderService
    {
        public readonly IRepository<Order> orderRepository;

        public OrderService(IRepository<Order> orderRepository)
        {
            this.orderRepository = orderRepository;
        }
        public Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> CreateAsync(BaseOrderModel creationModel)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }


    }
}
