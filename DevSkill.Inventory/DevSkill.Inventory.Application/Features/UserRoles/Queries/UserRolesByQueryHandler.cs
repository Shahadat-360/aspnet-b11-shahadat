using AutoMapper;
using DevSkill.Inventory.Application.Features.UserRoles.Queries;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.UserRoles.Queries
{
    public class UserRolesByQueryHandler(IRoleService roleService)
        : IRequestHandler<UserRolesByQuery, IList<UserRoleIndexDto>>
    {
        private readonly IRoleService _roleService = roleService;
        public async Task<IList<UserRoleIndexDto>> Handle(UserRolesByQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleService.GetAllRolesAsync();
            return roles;
        }
    }
}