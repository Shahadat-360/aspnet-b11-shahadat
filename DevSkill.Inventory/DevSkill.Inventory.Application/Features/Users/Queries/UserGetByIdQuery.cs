using DevSkill.Inventory.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Users.Queries
{
    public class UserGetByIdQuery:IRequest<UserDto>
    {
        public Guid Id { get; set; }
    }
}
