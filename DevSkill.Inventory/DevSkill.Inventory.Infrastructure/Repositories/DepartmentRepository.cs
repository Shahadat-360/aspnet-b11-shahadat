using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Enums;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Infrastructure;
using DevSkill.Inventory.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class DepartmentRepository(ApplicationDbContext applicationDbContext)
        : Repository<Department, int>(applicationDbContext), IDepartmentRepository
    {
        public Task<PaginatedResult<Department>> SearcDepartmenthWithPaginationAsync(string term, int page, int pageSize)
        {
            return SearchWithPaginationAsync(
                d => string.IsNullOrWhiteSpace(term) || d.Name.Contains(term),
                q => q.OrderBy(d => d.Status == Status.Inactive ? 1 : 0)
                      .ThenBy(d => d.Name),
                page,
                pageSize);
        }
    }
}