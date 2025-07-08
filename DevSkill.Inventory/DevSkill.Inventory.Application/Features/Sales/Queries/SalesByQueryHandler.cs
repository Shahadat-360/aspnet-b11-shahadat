using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Sales.Queries
{
    public class SalesByQueryHandler(IApplicationUnitOfWork  applicationUnitOfWork)
        : IRequestHandler<SalesByQuery, (IList<SalesIndexViewDto> data, int total, int totalDisplay)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<(IList<SalesIndexViewDto> data, int total, int totalDisplay)> Handle(SalesByQuery request, CancellationToken cancellationToken)
        {
            var order = request.FormatSortExpression("Id","Id", "OrderDate", "CustomerName", "Total", "Paid", "Due", "PaymentStatus");
            return await _applicationUnitOfWork.GetPagedSales(request.PageIndex,request.PageSize,order,request.SearchItem);
        }
    }
}
