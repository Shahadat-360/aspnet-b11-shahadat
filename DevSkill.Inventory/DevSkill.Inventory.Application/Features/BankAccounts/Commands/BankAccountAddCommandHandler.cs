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
    public class BankAccountAddCommandHandler (IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        : IRequestHandler<BankAccountAddCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task Handle(BankAccountAddCommand request, CancellationToken cancellationToken)
        {
            var bankAccount = _mapper.Map<BankAccount>(request);
            bankAccount.CurrentBalance = request.OpeningBalance; 
            await _applicationUnitOfWork.BankAccountRepository.AddAsync(bankAccount);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
