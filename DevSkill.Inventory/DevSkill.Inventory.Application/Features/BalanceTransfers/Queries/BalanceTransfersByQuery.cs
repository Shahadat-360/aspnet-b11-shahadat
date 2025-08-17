using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.BalanceTransfers.Queries
{
    public class BalanceTransfersByQuery: DataTables, IRequest<(IList<BalanceTransferIndexDto>, int, int)>
    {
        public BalanceTransferSearchDto? SearchItem { get; set; }
    }
}
