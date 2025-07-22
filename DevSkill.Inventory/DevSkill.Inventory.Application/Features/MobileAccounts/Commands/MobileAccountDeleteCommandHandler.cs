using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Enums;
using MediatR;

namespace DevSkill.Inventory.Application.Features.MobileAccounts.Commands
{
    public class MobileAccountDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
    : IRequestHandler<MobileAccountDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task Handle(MobileAccountDeleteCommand request, CancellationToken cancellationToken)
        {
            var existInsSales = await _applicationUnitOfWork.SaleRepository.AccountExistInSales(AccountType.Mobile, request.Id);
            if (existInsSales)
                throw new InvalidOperationException("Account is used in Sales");
            var mobileAccount = await _applicationUnitOfWork.MobileAccountRepository.GetByIdAsync(request.Id);
            await _applicationUnitOfWork.MobileAccountRepository.RemoveAsync(mobileAccount);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}