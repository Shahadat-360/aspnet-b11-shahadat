using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.MobileAccounts.Queries
{
    public class MobileAccountSearchWithPaginationQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        : IRequestHandler<MobileAccountSearchWithPaginationQuery, PaginatedResult<MobileAccount>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<PaginatedResult<MobileAccount>> Handle(MobileAccountSearchWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.MobileAccountRepository.SearchMobileAccounthWithPaginationAsync(request.term, request.page, request.pageSize);
        }
    }
}
