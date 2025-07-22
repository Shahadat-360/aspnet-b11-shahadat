using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Enums;
using MediatR;

namespace DevSkill.Inventory.Application.Features.CashAccounts.Commands
{
    internal class CashAccountDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
    : IRequestHandler<CashAccountDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task Handle(CashAccountDeleteCommand request, CancellationToken cancellationToken)
        {
            var existInsSales = await _applicationUnitOfWork.SaleRepository.AccountExistInSales(AccountType.Cash, request.Id);
            if (existInsSales)
                throw new InvalidOperationException("Account is used in Sales");
            var cashAccount = await _applicationUnitOfWork.CashAccountRepository.GetByIdAsync(request.Id);
            await _applicationUnitOfWork.CashAccountRepository.RemoveAsync(cashAccount);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}