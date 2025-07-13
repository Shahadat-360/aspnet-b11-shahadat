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
    public class MobileAccountRepository(ApplicationDbContext applicationDbContext) :
        Repository<MobileAccount, int>(applicationDbContext), IMobileAccountRepository
    {
        public async Task<PaginatedResult<MobileAccount>> SearchMobileAccounthWithPaginationAsync(string term, int page, int pageSize)
        {
            return await SearchWithPaginationAsync(
                m=>m.AccountName.Contains(term),
                q=>q.OrderBy(m=>m.Id),
                page, pageSize);
        }
    }
}
