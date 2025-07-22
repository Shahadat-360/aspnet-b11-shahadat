using AutoMapper;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Enums;
using MediatR;

namespace DevSkill.Inventory.Application.Features.BankAccounts.Commands
{
    public class BankAccountDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) 
        : IRequestHandler<BankAccountDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task Handle(BankAccountDeleteCommand request, CancellationToken cancellationToken)
        {
            var existInsSales = await _applicationUnitOfWork.SaleRepository.AccountExistInSales(AccountType.Bank, request.Id);
            if (existInsSales)
                throw new InvalidOperationException("Account is used in Sales");
            var bankAccount = await _applicationUnitOfWork.BankAccountRepository.GetByIdAsync(request.Id);
            await _applicationUnitOfWork.BankAccountRepository.RemoveAsync(bankAccount);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}