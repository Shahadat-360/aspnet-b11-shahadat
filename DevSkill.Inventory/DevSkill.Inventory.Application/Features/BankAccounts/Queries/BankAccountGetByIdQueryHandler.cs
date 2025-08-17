using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.BankAccounts.Queries
{
    public class BankAccountGetByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : IRequestHandler<BankAccountGetByIdQuery, BankAccount>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<BankAccount> Handle(BankAccountGetByIdQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.BankAccountRepository.GetByIdAsync(request.Id);
        }
    }
}
