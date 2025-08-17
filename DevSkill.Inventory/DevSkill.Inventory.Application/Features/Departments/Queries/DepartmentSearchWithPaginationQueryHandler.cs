using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Departments.Queries
{
    public class DepartmentSearchWithPaginationQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) :
        IRequestHandler<DepartmentSearchWithPaginationQuery, PaginatedResult<Department>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<PaginatedResult<Department>> Handle(DepartmentSearchWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.DepartmentRepository.SearcDepartmenthWithPaginationAsync(request.term, request.page, request.pageSize);
        }
    }
}
