using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Departments.Commands
{
    public class DepartmentDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) :
        IRequestHandler<DepartmentDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork; 
        public async Task Handle(DepartmentDeleteCommand request, CancellationToken cancellationToken)
        {
            var department = await _applicationUnitOfWork.DepartmentRepository.GetByIdAsync(request.Id);
            await _applicationUnitOfWork.DepartmentRepository.RemoveAsync(department);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
