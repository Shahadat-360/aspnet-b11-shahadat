using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.MobileAccounts.Queries
{
    public class MobileAccountGetByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) 
        : IRequestHandler<MobileAccountGetByIdQuery, MobileAccount>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<MobileAccount> Handle(MobileAccountGetByIdQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.MobileAccountRepository.GetByIdAsync(request.Id);
        }
    }
}
