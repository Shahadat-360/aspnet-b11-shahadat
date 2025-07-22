using AutoMapper;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.CashAccounts.Commands
{
    internal class CashAccountAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
    : IRequestHandler<CashAccountAddCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task Handle(CashAccountAddCommand request, CancellationToken cancellationToken)
        {
            var cashAccount = _mapper.Map<CashAccount>(request);
            cashAccount.CurrentBalance = request.OpeningBalance;
            await _applicationUnitOfWork.CashAccountRepository.AddAsync(cashAccount);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}