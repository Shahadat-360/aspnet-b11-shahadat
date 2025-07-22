using DevSkill.Inventory.Application.Features.Users.Commands;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure.Services
{
    public class UserService(ApplicationDbContext applicationDbContext, UserManager<ApplicationUser> userManager
        , RoleManager<ApplicationRole> roleManager) : IUserService
    {
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        public async Task AddUserAsync(UserAddCommand userAddCommand)
        {
            var employee = await _applicationDbContext.Employees
                .FirstOrDefaultAsync(e => e.Id == userAddCommand.EmployeeId) ?? 
                    throw new KeyNotFoundException($"Employee with ID '{userAddCommand.EmployeeId}' not found.");

            var role = await _roleManager.Roles
                .FirstOrDefaultAsync(r => r.Id == userAddCommand.RoleId) ?? 
                    throw new KeyNotFoundException($"Role with ID '{userAddCommand.RoleId}' not found.");
            
            var company = role.Company ?? 
                throw new KeyNotFoundException($"Company not found for role '{role.Name}'.");
            
            var newUser = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = employee.Email,
                Email = employee.Email,
                PhoneNumber = employee.Mobile,
                EmployeeId = userAddCommand.EmployeeId,
                Status = userAddCommand.Status,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(newUser, userAddCommand.Password);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            await _userManager.AddToRoleAsync(newUser, role.Name);
        }

        public async Task UpdateUserAsync(UserUpdateCommand userUpdateCommand)
        {
            var existingUser = await _userManager.FindByIdAsync(userUpdateCommand.Id.ToString()) ??
                throw new KeyNotFoundException($"User with ID '{userUpdateCommand.Id}' not found.");
            existingUser.Status = userUpdateCommand.Status;

            var updateResult = await _userManager.UpdateAsync(existingUser);
            if (!updateResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to update user: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");
            }

            if (!string.IsNullOrWhiteSpace(userUpdateCommand.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                var passwordResult = await _userManager.ResetPasswordAsync(existingUser, token, userUpdateCommand.Password);
                if (!passwordResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Failed to reset password: {string.Join(", ", passwordResult.Errors.Select(e => e.Description))}");
                }
            }

            var newRole = await _roleManager.FindByIdAsync(userUpdateCommand.RoleId.ToString());
            var currentRoles = await _userManager.GetRolesAsync(existingUser);
            if (newRole == null)
                throw new KeyNotFoundException($"Role with ID '{userUpdateCommand.RoleId}' not found.");

            if (!currentRoles.Contains(newRole.Name))
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);
                if (!removeResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Failed to remove user from current roles: {string.Join(", ", removeResult.Errors.Select(e => e.Description))}");
                }
                var addRoleResult = await _userManager.AddToRoleAsync(existingUser, newRole.Name);
                if (!addRoleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Failed to assign new role: {string.Join(", ", addRoleResult.Errors.Select(e => e.Description))}");
                }
            }
        }

        public async Task DeleteUserAsync(UserDeleteCommand request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
                throw new KeyNotFoundException($"User with ID '{request.Id}' not found.");

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Any())
            {
                var removeRoleResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
                if (!removeRoleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        $"Failed to remove user from roles: {string.Join(", ", removeRoleResult.Errors.Select(e => e.Description))}");
                }
            }

            var deleteResult = await _userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to delete user: {string.Join(", ", deleteResult.Errors.Select(e => e.Description))}");
            }
        }

        public async Task<UserDto> UserByIdAsync(Guid id)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == id);
            var employee = await _applicationDbContext.Employees
                .FirstOrDefaultAsync(e => e.Id == user.EmployeeId);
            var employeeName = employee.Name;
            var roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            var role = await _roleManager.FindByNameAsync(roleName);
            var roleId = role.Id;

            var dto = new UserDto
            {
                Id = user.Id,
                EmployeeId = user.EmployeeId,
                RoleId = roleId,
                Status = user.Status,
                EmployeeName= employeeName,
                RoleName = role.Name
            };
            return dto;
        }
    }
}