using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Departments.Queries
{
    public class DepartmentGetByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) 
        : IRequestHandler<DepartmentGetByIdQuery, Department>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<Department> Handle(DepartmentGetByIdQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.DepartmentRepository.GetByIdAsync(request.Id);
        }
    }
}