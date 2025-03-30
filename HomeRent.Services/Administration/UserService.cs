using AutoMapper;
using HomeRent.Data.Models.User;
using HomeRent.Models.Administration;
using HomeRent.Services.Administration.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeRent.Services.Administration
{
    public class UserService : IUserService
    {
        private readonly IMapper mapper;

        private readonly UserManager<ApplicationUser> userManager;

        public UserService(IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync()
        {
            var users = await this.userManager.Users
                .ToListAsync();

            return this.mapper.Map<IEnumerable<UserViewModel>>(users);
        }

        public async Task<UserDeleteViewModel> GetUserByIdAsync(string userId)
        {
            var user = await this.userManager.Users
                .FirstOrDefaultAsync(u => u.Id == new Guid(userId));

            if (user == null)
            {
                return null;
            }

            var roles = await this.userManager.GetRolesAsync(user);
            string role = roles.FirstOrDefault();

            var userModel = this.mapper.Map<UserDeleteViewModel>(user);
            userModel.Role = role;

            return userModel;
        }

        public async Task<bool> CanBeDeletedAsync(string userId)
        {
            var user = await this.userManager.Users
                .Include(u => u.OwnedProperties)
                .Include(u => u.Bookings)
                .Include(u => u.Reviews)
                .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            if (user == null)
                return false;

            var roles = await this.userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();

            return role switch
            {
                "Tenant" => (user.Bookings == null || !user.Bookings.Any()) &&
                            (user.Reviews == null || !user.Reviews.Any()),
                "Owner" => user.OwnedProperties == null || !user.OwnedProperties.Any(),
                _ => false
            };
        }

        public async Task DeleteUserAsync(string userId)
        {
            var user = await this.userManager.Users
                .FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            if (user != null)
            {
                user.IsDeleted = true;
                user.DeletedOn = DateTime.UtcNow;

                await this.userManager.UpdateAsync(user);
            }
        }
    }
}
