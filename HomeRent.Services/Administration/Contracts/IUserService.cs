using HomeRent.Models.Administration;

namespace HomeRent.Services.Administration.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserViewModel>> GetAllUsersAsync();
    }
}
