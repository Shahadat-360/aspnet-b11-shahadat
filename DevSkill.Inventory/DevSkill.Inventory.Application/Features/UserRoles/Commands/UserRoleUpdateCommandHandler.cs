using AutoMapper;
using DevSkill.Inventory.Application.Features.Categories.Commands;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.UserRoles.Commands
{
    public class UserRoleUpdateCommandHandler(IRoleService roleService)
        : IRequestHandler<UserRoleUpdateCommand>
    {
        private readonly IRoleService _roleService = roleService;
        public async Task Handle(UserRoleUpdateCommand request, CancellationToken cancellationToken)
        {
            await _roleService.UpdateRoleAsync(request);
        }
    }
}