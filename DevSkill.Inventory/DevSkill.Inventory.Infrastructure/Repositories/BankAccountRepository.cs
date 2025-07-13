using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class BankAccountRepository(ApplicationDbContext applicationDbContext)
        : Repository<BankAccount, int>(applicationDbContext), IBankAccountRepository
    {
        public async Task<PaginatedResult<BankAccount>> SearchBankAccounthWithPaginationAsync(string term, int page, int pageSize)
        {
            return await SearchWithPaginationAsync(
                b=>b.AccountName.Contains(term),
                q=>q.OrderBy(b=>b.Id),
                page, pageSize);
        }
    }
}
