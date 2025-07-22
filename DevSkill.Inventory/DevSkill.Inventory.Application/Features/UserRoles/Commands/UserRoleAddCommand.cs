using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.UserRoles.Commands
{
    public class UserRoleAddCommand : IRequest
    {
        public string Name { get; set; }
        public Status Status { get; set; } = Status.Active;
    }
}
