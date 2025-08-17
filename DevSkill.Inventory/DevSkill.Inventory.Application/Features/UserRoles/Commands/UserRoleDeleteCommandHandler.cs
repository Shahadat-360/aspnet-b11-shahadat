using DevSkill.Inventory.Application.Services;
using MediatR;

namespace DevSkill.Inventory.Application.Features.UserRoles.Commands
{
    public class UserRoleDeleteCommandHandler(IRoleService roleService) :
        IRequestHandler<UserRoleDeleteCommand>
    {
        private readonly IRoleService _roleService = roleService;
        public async Task Handle(UserRoleDeleteCommand request, CancellationToken cancellationToken)
        {
            await _roleService.DeleteRoleAsync(request.Id);
        }
    }
}
