using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.BalanceTransfers.Commands
{
    public class BalanceTransferDeleteCommand:IRequest
    {
        public Guid Id { get; set; }
    }
}
