using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Services
{
    public class AccessSetupService(RoleManager<ApplicationRole> roleManager,
    ApplicationDbContext context) : IAccessSetupService
    {
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly ApplicationDbContext _context = context;
        public async Task<List<RoleClaimsDto>> GetRoleClaimsAsync(Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null) return new();

            var assignedClaims = (await _roleManager.GetClaimsAsync(role)).ToList();

            var allClaims = await _context.RoleClaims
                .Select(c => new { c.ClaimType, c.ClaimValue })
                .Distinct()
                .ToListAsync();

            return allClaims.Select(c => new RoleClaimsDto
            {
                RoleId = roleId.ToString(),
                ClaimType = c.ClaimType,
                ClaimValue = c.ClaimValue,
                Selected = assignedClaims.Any(ac => ac.Type == c.ClaimType && ac.Value == c.ClaimValue)
            }).ToList();
        }

        public async Task<List<AccessSetupIndexDto>> GetRolesAsync()
        {
            return await _roleManager.Roles
                .Select(r => new AccessSetupIndexDto
                {
                    RoleId = r.Id,
                    RoleName = r.Name,
                    Status = r.Status
                })
                .ToListAsync();
        }
        public async Task<bool> UpdateRoleClaimsAsync(string roleId, List<string> selectedClaims)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return false;

            var currentClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in currentClaims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }

            var allClaimsInDb = await _context.RoleClaims
                .Select(c => new { c.ClaimType, c.ClaimValue })
                .Distinct()
                .ToListAsync();

            foreach (var selectedClaim in selectedClaims)
            {
                var claimInfo = allClaimsInDb.FirstOrDefault(c => c.ClaimValue == selectedClaim);
                if (claimInfo != null)
                {
                    await _roleManager.AddClaimAsync(role, new Claim(claimInfo.ClaimType, claimInfo.ClaimValue));
                }
            }

            return true;
        }
    }
}