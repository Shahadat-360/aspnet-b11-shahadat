using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Enums;
using DevSkill.Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class EmployeeRepository(ApplicationDbContext applicationDbContext) :
        Repository<Employee, string>(applicationDbContext), IEmployeeRepository
    {
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
        public async Task<Employee> GetByIdWithNavigationAsync(string id)
        {
            var emp = await _applicationDbContext.Employees.Where(e=>e.Id==id).Include(e=>e.Department).SingleOrDefaultAsync();
            return emp;
        }

        public async Task<PaginatedResult<Employee>> SearchEmployeeWithPaginationAsync(string term, int page, int pageSize)
        {
            return await SearchWithPaginationAsync(
                e => (string.IsNullOrWhiteSpace(term) || e.Name.Contains(term))
                 && !_applicationDbContext.Users.Any(u=>u.EmployeeId==e.Id),
                q => q.OrderBy(e => e.Status == Status.Inactive ? 1 : 0)
                      .ThenBy(e => e.Name),
                page,
                pageSize);
        }
    }
}
