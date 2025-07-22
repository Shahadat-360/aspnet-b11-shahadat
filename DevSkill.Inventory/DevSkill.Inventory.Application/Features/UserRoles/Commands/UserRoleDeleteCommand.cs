using MediatR;

namespace DevSkill.Inventory.Application.Features.UserRoles.Commands
{
    public class UserRoleDeleteCommand:IRequest
    {
        public Guid Id { get; set; }
    }
}
