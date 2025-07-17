using DevSkill.Inventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public interface IBalanceTransferService
    {
        Task<decimal?> GetAccountCurrentBalanceAsync(AccountType accountType, int accountId);
        Task UpdateAccountBalanceAsync(AccountType accountType, int accountId, decimal newBalance);
    }
}
