using DevSkill.Inventory.Application.Features.UserRoles.Commands;
using DevSkill.Inventory.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public interface IRoleService
    {
        Task<IList<UserRoleIndexDto>> GetAllRolesAsync();
        Task<UserRoleDto> GetRoleByIdAsync(Guid Id);
        Task AddRoleAsync(UserRoleAddCommand userRoleAddCommand);
        Task UpdateRoleAsync(UserRoleUpdateCommand userRoleUpdateCommand);
        Task DeleteRoleAsync(Guid Id);
        Task<PaginatedResult<UserRoleDto>> SearchUserRoleWithPaginationAsync(string term, int page, int pageSize);
    }
}
