using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface IUnitRepository : IRepository<Unit, Guid>
    {
        Task<PaginatedResult<Unit>> SearchUnitWithPaginationAsync(string term, int page, int pageSize);
    }
}
