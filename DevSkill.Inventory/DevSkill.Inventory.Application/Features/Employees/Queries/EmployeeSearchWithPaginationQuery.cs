using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Employees.Queries
{
    public class EmployeeSearchWithPaginationQuery : IRequest<PaginatedResult<Employee>>
    {
        public string term { get; set; } = string.Empty;
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 5;
    }
}