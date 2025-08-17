using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Employees.Queries
{
    public class EmployeesByQueryHandler(IApplicationUnitOfWork applicationUnitOfWork):IRequestHandler<EmployeesByQuery, (IList<EmployeeDto> Employees, int total, int totalDisplay)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<(IList<EmployeeDto> Employees, int total, int totalDisplay)> Handle(EmployeesByQuery request, CancellationToken cancellationToken)
        {
        string? order = request.FormatSortExpression("ID","Id","Name", "Mobile", "Email", "Address", "JoiningDate", "Salary", "Status");
            return await _applicationUnitOfWork.GetPagedEmployees(request.PageIndex, request.PageSize, order, request.SearchItem);
        }
    }
}
