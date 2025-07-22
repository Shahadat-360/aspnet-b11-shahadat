using AutoMapper;
using DevSkill.Inventory.Domain;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Employees.Commands
{
    public class EmployeeUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,
        IMapper mapper) : IRequestHandler<EmployeeUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task Handle(EmployeeUpdateCommand request, CancellationToken cancellationToken)
        {
            var employee = await _applicationUnitOfWork.EmployeeRepository.GetByIdAsync(request.Id);
            _mapper.Map(request, employee);
            await _applicationUnitOfWork.EmployeeRepository.UpdateAsync(employee);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
