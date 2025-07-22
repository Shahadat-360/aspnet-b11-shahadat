using AutoMapper;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Departments.Commands
{
    public class DepartmentAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,IMapper mapper) 
        : IRequestHandler<DepartmentAddCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task Handle(DepartmentAddCommand request, CancellationToken cancellationToken)
        {
            var department = _mapper.Map<Department>(request);
            await _applicationUnitOfWork.DepartmentRepository.AddAsync(department);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
