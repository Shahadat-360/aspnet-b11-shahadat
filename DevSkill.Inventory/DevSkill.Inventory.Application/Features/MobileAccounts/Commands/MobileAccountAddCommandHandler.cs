using AutoMapper;
using DevSkill.Inventory.Application.Features.BankAccounts.Commands;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.MobileAccounts.Commands
{
    public class MobileAccountAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
    : IRequestHandler<MobileAccountAddCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task Handle(MobileAccountAddCommand request, CancellationToken cancellationToken)
        {
            var mobileAccount = _mapper.Map<MobileAccount>(request);
            mobileAccount.CurrentBalance = request.OpeningBalance;
            await _applicationUnitOfWork.MobileAccountRepository.AddAsync(mobileAccount);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}