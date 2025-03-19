using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DTOs;
using BusinessLogic.Models.PurchaseModels;

namespace BusinessLogic.Interfaces
{
    public interface IPurchaseService
    {
        Task<IEnumerable<PurchaseDto>> GetAllAsync();
        Task<PurchaseDto> GetByIdAsync(int id);
        Task<PurchaseDto> CreateAsync(BasePurchaseModel creationModel, int userId);
        Task DeleteAsync(int id);
    }
}
