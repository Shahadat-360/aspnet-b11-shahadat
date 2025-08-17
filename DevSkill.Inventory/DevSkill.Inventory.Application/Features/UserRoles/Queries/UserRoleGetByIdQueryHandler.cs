using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.UserRoles.Queries
{
    public class UserRoleGetByIdQueryHandler(IRoleService roleService) 
        : IRequestHandler<UserRoleGetByIdQuery, UserRoleDto>
    {
        private readonly IRoleService _roleService = roleService;
        public async Task<UserRoleDto> Handle(UserRoleGetByIdQuery request, CancellationToken cancellationToken)
        {
            return await _roleService.GetRoleByIdAsync(request.Id);
        }
    }
}