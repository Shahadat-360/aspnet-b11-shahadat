using DevSkill.Inventory.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.CashAccounts.Queries
{
    public class CashAccountsByQuery: IRequest<IList<CashAccountIndexDto>>
    {
    }
}
