using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Enums;
using MediatR;
using Unit = DevSkill.Inventory.Domain.Entities.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Units.Commands
{
    public class UnitAddCommand: IRequest<Unit>
    {
        public string Name { get; set; }
        public Status Status { get; set; } = Status.Active;
    }
}
