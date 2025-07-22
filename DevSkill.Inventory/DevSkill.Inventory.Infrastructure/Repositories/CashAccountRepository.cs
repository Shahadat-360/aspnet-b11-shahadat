using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class CashAccountRepository(ApplicationDbContext applicationDbContext)
        : Repository<CashAccount, int>(applicationDbContext), ICashAccountRepository
    {
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
        public async Task<CashAccount> GetByIdAsNoTrackingAsync(int id)
        {
            return await _applicationDbContext.CashAccounts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<PaginatedResult<CashAccount>> SearchCashAccounthWithPaginationAsync(string term, int page, int pageSize)
        {
            return await SearchWithPaginationAsync(
                c=>c.AccountName.Contains(term),
                q=>q.OrderBy(c=>c.Id),
                page, pageSize);
        }
    }
}
