using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.MobileAccounts.Queries
{
    public class MobileAccountGetByIdQuery:IRequest<MobileAccount>
    {
        public int Id { get; set; }
    }
}
