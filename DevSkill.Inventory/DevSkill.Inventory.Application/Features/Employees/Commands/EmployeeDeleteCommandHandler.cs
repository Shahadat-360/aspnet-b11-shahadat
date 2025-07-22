using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Employees.Commands
{
    public class EmployeeDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) 
        : IRequestHandler<EmployeeDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task Handle(EmployeeDeleteCommand request, CancellationToken cancellationToken)
        {
            await _applicationUnitOfWork.EmployeeRepository.RemoveAsync(request.Id);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}