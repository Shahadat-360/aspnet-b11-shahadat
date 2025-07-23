using AutoMapper;
using DevSkill.Inventory.Application.Features.UserRoles.Commands;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Services
{
    public class RoleService(RoleManager<ApplicationRole> roleManager, IMapper mapper
        , ITransactionService transactionService) : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly IMapper _mapper = mapper;
        private readonly ITransactionService _transactionService = transactionService;
        public async Task AddRoleAsync(UserRoleAddCommand userRoleAddCommand)
        {
            var newRole = _mapper.Map<ApplicationRole>(userRoleAddCommand);
            newRole.ConcurrencyStamp = Guid.NewGuid().ToString();
            var result = await _roleManager.CreateAsync(newRole);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to create role: {string.Join(", ", result.Errors)}");
            }
        }

        public async Task DeleteRoleAsync(Guid id)
        {
            await _transactionService.BeginTransactionAsync();
            try
            {
                var role = await _roleManager.FindByIdAsync(id.ToString());
                if (role == null)
                    throw new KeyNotFoundException($"Role with ID '{id}' not found.");
                var claims = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claims)
                {
                    var removeResult = await _roleManager.RemoveClaimAsync(role, claim);
                    if (!removeResult.Succeeded)
                    {
                        throw new InvalidOperationException(
                            $"Failed to remove claim '{claim.Type}': {string.Join(", ", removeResult.Errors.Select(e => e.Description))}");
                    }
                }
                var result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Failed to delete role: {string.Join(", ", result.Errors)}");
                }
                await _transactionService.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackTransactionAsync();
                throw new InvalidOperationException("An error occurred while deleting the role.", ex);
            }
        }

        public async Task<IList<UserRoleIndexDto>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return _mapper.Map<IList<UserRoleIndexDto>>(roles);
        }

        public async Task<UserRoleDto> GetRoleByIdAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
                throw new KeyNotFoundException($"Role with ID '{id}' not found.");

            return _mapper.Map<UserRoleDto>(role);
        }

        public async Task<PaginatedResult<UserRoleDto>> SearchUserRoleWithPaginationAsync(string term, int page, int pageSize)
        {
            var query = _roleManager.Roles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(r => r.Name.Contains(term));
            }

            var totalCount = query.Count();
            var roles = await query
                .OrderBy(r => r.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new UserRoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Status = r.Status
                })
                .ToListAsync();

            return PaginatedResult<UserRoleDto>.Create(roles, totalCount, page, pageSize);
        }

        public async Task UpdateRoleAsync(UserRoleUpdateCommand userRoleUpdateCommand)
        {
            var role = await _roleManager.FindByIdAsync(userRoleUpdateCommand.Id.ToString());
            if (role == null)
                throw new KeyNotFoundException($"Role with ID '{userRoleUpdateCommand.Id}' not found.");
            _mapper.Map(userRoleUpdateCommand, role);
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to update role: {string.Join(", ", result.Errors)}");
            }
        }
    }
}
