using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Employees.Queries
{
    public class EmployeeGetByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : IRequestHandler<EmployeeGetByIdQuery, Employee>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<Employee> Handle(EmployeeGetByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await _applicationUnitOfWork.EmployeeRepository.GetByIdWithNavigationAsync(request.Id);
            return employee == null ? throw new KeyNotFoundException($"Employee with ID {request.Id} not found.") : employee;
        }
    }
}
