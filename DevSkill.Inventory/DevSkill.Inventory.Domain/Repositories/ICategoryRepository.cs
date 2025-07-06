using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface ICategoryRepository:IRepository<Category, Guid>
    {
        Task<PaginatedResult<Category>> SearcCategoryhWithPaginationAsync(string term, int page, int pageSize);
    }
}
