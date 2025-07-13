using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface ICustomerRepository : IRepository<Customer, string>
    {
        Task<PaginatedResult<Customer>> SearchCustomerWithPaginationAsync(string term, int page, int pageSize);
    }
}
