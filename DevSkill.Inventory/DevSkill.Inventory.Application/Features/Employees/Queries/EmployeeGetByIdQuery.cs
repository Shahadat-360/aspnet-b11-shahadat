using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Employees.Queries
{
    public class EmployeeGetByIdQuery:IRequest<Employee>
    {
        public string Id { get; set; }
    }
}
