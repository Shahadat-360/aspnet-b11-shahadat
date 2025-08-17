using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.BalanceTransfers.Queries
{
    public class BalanceTransfersByQueryHandler (IApplicationUnitOfWork applicationUnitOfWork)
        : IRequestHandler<BalanceTransfersByQuery, (IList<BalanceTransferIndexDto>, int, int)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<(IList<BalanceTransferIndexDto>, int, int)> Handle(BalanceTransfersByQuery request, CancellationToken cancellationToken)
        {
            string? order = request.FormatSortExpression("TransferDate", "TransferDate", "TransferDate", "SendingAccount", "TransferDate", "ReceivingAccount", "TransferAmount","Note");
            return await _applicationUnitOfWork.GetPagedBalanceTransfers(request.PageIndex, request.PageSize, order, request.SearchItem);
        }
    }
}
