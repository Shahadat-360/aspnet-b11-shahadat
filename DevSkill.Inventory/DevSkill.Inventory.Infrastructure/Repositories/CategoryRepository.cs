using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Enums;
using DevSkill.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class CategoryRepository(ApplicationDbContext applicationDbContext) :
        Repository<Category, Guid>(applicationDbContext), ICategoryRepository
    {
        public async Task<PaginatedResult<Category>> SearcCategoryhWithPaginationAsync(string term, int page, int pageSize)
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
