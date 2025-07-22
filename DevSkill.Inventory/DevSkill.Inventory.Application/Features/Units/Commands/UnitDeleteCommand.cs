using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Units.Commands
{
    public class UnitDeleteCommand:IRequest
    {
        public Guid Id { get; set; }
    }
}
