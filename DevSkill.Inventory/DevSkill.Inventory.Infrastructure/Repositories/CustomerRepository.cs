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
    public class CustomerRepository(ApplicationDbContext applicationDbContext) :
        Repository<Customer, string>(applicationDbContext), ICustomerRepository
    {
        public async Task<Customer> GetByIdWithNavigationAsync(string Id)
        {
            var customers = await GetAsync(
                c => c.Id == Id,
                q => q.Include(c => c.Sales)
                );
            return customers.Single();
        }
        public async Task<PaginatedResult<Customer>> SearchCustomerWithPaginationAsync(string term, int page, int pageSize)
        {
            return await SearchWithPaginationAsync(
                c => string.IsNullOrWhiteSpace(term) || c.CustomerName.Contains(term) || c.Mobile.Contains(term),
                q => q.OrderBy(c => c.Id), page, pageSize);
        }
    }
}
