using BusinessLogic.Entities;
using BusinessLogic.Models.UserModels;

namespace BusinessLogic.Interfaces
{
    public interface IAccountService
    {
        Task AddAsync(BaseUserModel createModel);
        Task DeleteAsync(int userId);
        Task DeleteAsync(User user);
    }
}
