using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DTOs;
using BusinessLogic.Models.BalanceModels;

namespace BusinessLogic.Interfaces
{
    public interface IBalanceService
    {
        Task<IEnumerable<BalanceDto>> GetAllAsync();
        Task<BalanceDto> GetByIdAsync(int id);
        Task<BalanceDto> CreateAsync(BaseBalanceModel creationModel);
        Task DeleteAsync(int id);
    }
}
