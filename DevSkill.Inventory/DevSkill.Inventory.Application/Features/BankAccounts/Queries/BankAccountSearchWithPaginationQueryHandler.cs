using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.BankAccounts.Queries
{
    public class BankAccountSearchWithPaginationQueryHandler (IApplicationUnitOfWork applicationUnitOfWork)
        : IRequestHandler<BankAccountSearchWithPaginationQuery, PaginatedResult<BankAccount>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<PaginatedResult<BankAccount>> Handle(BankAccountSearchWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.BankAccountRepository.SearchBankAccounthWithPaginationAsync(request.term,request.page,request.pageSize);
        }
    }
}
