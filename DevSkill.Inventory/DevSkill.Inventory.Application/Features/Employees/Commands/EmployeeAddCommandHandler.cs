using AutoMapper;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Employees.Commands
{
    public class EmployeeAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,IMapper mapper,
        IIdGenerator idGenerator) : 
        IRequestHandler<EmployeeAddCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IIdGenerator _idGenerator = idGenerator;
        public async Task Handle(EmployeeAddCommand request, CancellationToken cancellationToken)
        {
            var employee = _mapper.Map<Employee>(request);
            employee.Id = await _idGenerator.GenerateIdAsync("E-DEV");
            await _applicationUnitOfWork.EmployeeRepository.AddAsync(employee);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
