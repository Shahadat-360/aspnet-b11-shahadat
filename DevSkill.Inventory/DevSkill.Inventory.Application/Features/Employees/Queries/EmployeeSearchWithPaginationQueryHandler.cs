using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Employees.Queries
{
    public class EmployeeSearchWithPaginationQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        : IRequestHandler<EmployeeSearchWithPaginationQuery, PaginatedResult<Employee>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;

        public async Task<PaginatedResult<Employee>> Handle(EmployeeSearchWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.EmployeeRepository
                .SearchEmployeeWithPaginationAsync(request.term, request.page, request.pageSize);
        }
    }
}