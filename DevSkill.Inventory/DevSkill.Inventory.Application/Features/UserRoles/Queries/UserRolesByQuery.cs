using DevSkill.Inventory.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.UserRoles.Queries
{
    public class UserRolesByQuery:IRequest<IList<UserRoleIndexDto>>
    {
    }
}
