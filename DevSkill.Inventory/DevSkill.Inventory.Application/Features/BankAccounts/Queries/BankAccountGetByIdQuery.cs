using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.BankAccounts.Queries
{
    public class BankAccountGetByIdQuery:IRequest<BankAccount>
    {
        public int Id { get; set; }
    }
}
