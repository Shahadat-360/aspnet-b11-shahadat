using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Enums;
using DevSkill.Inventory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class UnitRepository(ApplicationDbContext applicationDbContext)
        : Repository<Unit, Guid>(applicationDbContext), IUnitRepository
    {
        //private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
        //public Task<PaginatedResult<Unit>> SearchWithPaginationAsync(string term, int page = 1, int pageSize = 5)
        //{
        //    var query = _applicationDbContext.Units.AsQueryable();
        //    if (!string.IsNullOrWhiteSpace(term))
        //    {
        //        query = query.Where(c => c.Name.Contains(term));
        //    }
        //    var totalCount = query.Count();
        //    query = query.OrderBy(c => c.Status == Status.Inactive ? 1 : 0)
        //                 .ThenBy(c => c.Name);
        //    var items = query.Skip((page - 1) * pageSize)
        //                     .Take(pageSize)
        //                     .ToList();
        //    return Task.FromResult(new PaginatedResult<Unit>
        //    {
        //        Items = items,
        //        TotalCount = totalCount,
        //        Page = page,
        //        PageSize = pageSize
        //    });
        //}
        public async Task<PaginatedResult<Unit>> SearchUnitWithPaginationAsync(string term, int page, int pageSize)
        {
            return await SearchWithPaginationAsync(
                c => string.IsNullOrWhiteSpace(term) || c.Name.Contains(term),
                q => q.OrderBy(c => c.Status == Status.Inactive ? 1 : 0)
                      .ThenBy(c => c.Name),
                page,
                pageSize);
        }
    }
}
