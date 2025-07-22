using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.UserRoles.Queries
{
    public class UserRoleGetByIdQuery:IRequest<UserRoleDto>
    {
        public Guid Id { get; set; }
    }
}
