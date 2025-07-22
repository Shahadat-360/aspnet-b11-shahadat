using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Departments.Queries
{
    public class DepartmentSearchWithPaginationQuery : IRequest<PaginatedResult<Department>>
    {
        public string term=string.Empty;
        public int page = 1; 
        public int pageSize = 5;
    }
}
