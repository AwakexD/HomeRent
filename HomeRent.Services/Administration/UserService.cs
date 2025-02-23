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
                .Include(u => u.Roles)
                .ToListAsync();

            return this.mapper.Map<IEnumerable<UserViewModel>>(users);
        }
    }
}
