using DevSkill.Inventory.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public interface IAccessSetupService
    {
        Task<List<AccessSetupIndexDto>> GetRolesAsync();
        Task<List<RoleClaimsDto>> GetRoleClaimsAsync(Guid roleId);
        Task<bool> UpdateRoleClaimsAsync(string roleId, List<string> selectedClaims);
    }
}
