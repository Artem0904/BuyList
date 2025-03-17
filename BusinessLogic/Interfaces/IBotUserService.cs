using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.DTOs;
using BusinessLogic.Entities;

namespace BusinessLogic.Interfaces
{
    public interface IBotUserService
    {
        Task<IEnumerable<BotUserDto>> GetAllAsync();
        Task<BotUserDto> GetByIdAsync(int userId);
        Task<BotUserDto> GetByChatIdAsync(long chatId);
        Task DeleteAsync(long chatId);
    }
}
