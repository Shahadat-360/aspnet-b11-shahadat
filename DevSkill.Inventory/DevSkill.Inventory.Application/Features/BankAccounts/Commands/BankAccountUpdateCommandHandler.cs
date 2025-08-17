using AutoMapper;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.BankAccounts.Commands
{
    public class BankAccountUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        : IRequestHandler<BankAccountUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task Handle(BankAccountUpdateCommand request, CancellationToken cancellationToken)
        {
            var oldBankAccount = await _applicationUnitOfWork.BankAccountRepository.GetByIdAsNoTrackingAsync(request.Id);
            var newBankAccount = _mapper.Map<BankAccount>(request);
            newBankAccount.CurrentBalance = oldBankAccount.CurrentBalance + (request.OpeningBalance - oldBankAccount.OpeningBalance);
            await _applicationUnitOfWork.BankAccountRepository.UpdateAsync(newBankAccount);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
