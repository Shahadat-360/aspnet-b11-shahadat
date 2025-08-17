using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.CashAccounts.Queries
{
    public class CashAccountGetByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : IRequestHandler<CashAccountGetByIdQuery, CashAccount>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<CashAccount> Handle(CashAccountGetByIdQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.CashAccountRepository.GetByIdAsync(request.Id);
        }
    }
}
