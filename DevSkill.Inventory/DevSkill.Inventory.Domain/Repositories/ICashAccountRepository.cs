using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface ICashAccountRepository:IRepository<CashAccount, int>
    {
        Task<CashAccount> GetByIdAsNoTrackingAsync(int id);
        Task<PaginatedResult<CashAccount>> SearchCashAccounthWithPaginationAsync(string term, int page,int pageSize);
    }
}
