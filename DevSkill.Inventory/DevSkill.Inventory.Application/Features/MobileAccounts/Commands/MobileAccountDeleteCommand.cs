using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.MobileAccounts.Commands
{
    public class MobileAccountDeleteCommand:IRequest
    {
        public int Id { get; set; }
    }
}
