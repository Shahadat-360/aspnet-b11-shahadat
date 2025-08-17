using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.CashAccounts.Queries
{
    public class CashAccountSearchWithPaginationQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) 
        : IRequestHandler<CashAccountSearchWithPaginationQuery, PaginatedResult<CashAccount>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<PaginatedResult<CashAccount>> Handle(CashAccountSearchWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.CashAccountRepository.SearchCashAccounthWithPaginationAsync(request.term,request.page,request.pageSize);
        }
    }
}
