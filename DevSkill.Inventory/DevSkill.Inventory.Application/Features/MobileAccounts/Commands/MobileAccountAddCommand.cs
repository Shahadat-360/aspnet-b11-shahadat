using DevSkill.Inventory.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.MobileAccounts.Commands
{
    public class MobileAccountAddCommand:IRequest
    {
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string AccountOwner { get; set; }
        public decimal OpeningBalance { get; set; }
        public Status Status { get; set; } = Status.Active;
    }
}
