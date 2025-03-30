using HomeRent.Models.Administration;

namespace HomeRent.Services.Administration.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserViewModel>> GetAllUsersAsync();

        Task<UserDeleteViewModel> GetUserByIdAsync(string userId);

        Task<bool> CanBeDeletedAsync(string userId);

        Task DeleteUserAsync(string userId);
    }
}
