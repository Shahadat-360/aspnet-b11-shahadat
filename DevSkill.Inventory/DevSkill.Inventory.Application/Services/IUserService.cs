using DevSkill.Inventory.Application.Features.Users.Commands;
using DevSkill.Inventory.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public interface IUserService
    {
        Task AddUserAsync(UserAddCommand userAddCommand);
        Task DeleteUserAsync(UserDeleteCommand request);
        Task UpdateUserAsync(UserUpdateCommand userUpdateCommand);
        Task<UserDto> UserByIdAsync(Guid id);
    }
}
