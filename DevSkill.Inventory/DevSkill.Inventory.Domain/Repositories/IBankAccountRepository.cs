using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface IBankAccountRepository : IRepository<BankAccount, int>
    {
        Task<PaginatedResult<BankAccount>> SearchBankAccounthWithPaginationAsync(string term,int page,int pageSize);
    }
}
