using AutoMapper;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.CashAccounts.Commands
{
    public class CashAccountUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
    : IRequestHandler<CashAccountUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task Handle(CashAccountUpdateCommand request, CancellationToken cancellationToken)
        {
            var oldCashAccount = await _applicationUnitOfWork.CashAccountRepository.GetByIdAsNoTrackingAsync(request.Id);
            var newCashAccount = _mapper.Map<CashAccount>(request);
            newCashAccount.CurrentBalance = oldCashAccount.CurrentBalance + (request.OpeningBalance - oldCashAccount.OpeningBalance);
            await _applicationUnitOfWork.CashAccountRepository.UpdateAsync(newCashAccount);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}