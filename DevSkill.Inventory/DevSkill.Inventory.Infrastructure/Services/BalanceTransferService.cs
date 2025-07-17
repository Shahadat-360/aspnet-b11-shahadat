using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Services
{
    public class BalanceTransferService(IApplicationUnitOfWork applicationUnitOfWork) 
        : IBalanceTransferService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<decimal?> GetAccountCurrentBalanceAsync(AccountType accountType, int accountId)
        {
            return accountType switch
            {
                AccountType.Cash => (await _applicationUnitOfWork.CashAccountRepository
                    .GetByIdAsync(accountId))?.CurrentBalance,

                AccountType.Bank => (await _applicationUnitOfWork.BankAccountRepository
                    .GetByIdAsync(accountId))?.CurrentBalance,

                AccountType.Mobile => (await _applicationUnitOfWork.MobileAccountRepository
                    .GetByIdAsync(accountId))?.CurrentBalance,

                _ => throw new ArgumentException($"Unsupported account type: {accountType}")
            };
        }

        public async Task UpdateAccountBalanceAsync(AccountType accountType, int accountId, decimal newBalance)
        {
            switch (accountType)
            {
                case AccountType.Cash:
                    var cashAccount = await _applicationUnitOfWork.CashAccountRepository.GetByIdAsync(accountId);
                    if (cashAccount != null)
                    {
                        cashAccount.CurrentBalance = newBalance;
                        await _applicationUnitOfWork.CashAccountRepository.UpdateAsync(cashAccount);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Cash account with ID {accountId} not found.");
                    }
                    break;

                case AccountType.Bank:
                    var bankAccount = await _applicationUnitOfWork.BankAccountRepository.GetByIdAsync(accountId);
                    if (bankAccount != null)
                    {
                        bankAccount.CurrentBalance = newBalance;
                        await _applicationUnitOfWork.BankAccountRepository.UpdateAsync(bankAccount);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Bank account with ID {accountId} not found.");
                    }
                    break;

                case AccountType.Mobile:
                    var mobileAccount = await _applicationUnitOfWork.MobileAccountRepository.GetByIdAsync(accountId);
                    if (mobileAccount != null)
                    {
                        mobileAccount.CurrentBalance = newBalance;
                        await _applicationUnitOfWork.MobileAccountRepository.UpdateAsync(mobileAccount);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Mobile account with ID {accountId} not found.");
                    }
                    break;

                default:
                    throw new ArgumentException($"Unsupported account type: {accountType}");
            }
        }
    }
}
