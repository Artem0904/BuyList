using BusinessLogic.Entities;
using BusinessLogic.Models.UserModels;

namespace BusinessLogic.Interfaces
{
    public interface IAccountService
    {
        Task AddAsync(BaseBotUserModel createModel);
        Task DeleteAsync(int userId);
        Task DeleteAsync(BotUser user);
    }
}
