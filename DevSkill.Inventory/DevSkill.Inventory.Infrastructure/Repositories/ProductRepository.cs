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
    public class ProductRepository(ApplicationDbContext dbContext)
        : Repository<Product, string>(dbContext), IProductRepository
    {
        public async Task<Product> GetProductWithNavigationAsync(string Id)
        {
            var products = await GetAsync(
                p => p.Id == Id, 
                q => q.Include(p => p.Category)
                    .Include(p => p.Unit));
            return products.Single();
        }

        public async Task<PaginatedResult<Product>> SearchProductWithPaginationAsync(string term, int page, int pageSize)
        {
            return await SearchWithPaginationAsync(
                p => string.IsNullOrWhiteSpace(term) || p.Name.Contains(term) || p.Id.Contains(term),
                q => q.OrderBy(p => p.Id),page,pageSize);
        }
    }
}
