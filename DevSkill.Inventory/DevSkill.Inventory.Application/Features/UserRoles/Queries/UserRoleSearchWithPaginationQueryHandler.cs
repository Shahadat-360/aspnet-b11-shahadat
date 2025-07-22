using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.UserRoles.Queries
{
    public class UserRoleSearchWithPaginationQueryHandler(IRoleService roleService)
        : IRequestHandler<UserRoleSearchWithPaginationQuery, PaginatedResult<UserRoleDto>>
    {
        private readonly IRoleService _roleService = roleService;
        public async Task<PaginatedResult<UserRoleDto>> Handle(UserRoleSearchWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _roleService.SearchUserRoleWithPaginationAsync(request.term, request.page, request.pageSize);
        }
    }
}