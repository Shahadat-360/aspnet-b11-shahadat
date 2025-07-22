using AutoMapper;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Departments.Commands
{
    public class DepartmentUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        : IRequestHandler<DepartmentUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task Handle(DepartmentUpdateCommand request, CancellationToken cancellationToken)
        {
            var UpdatedDepartment = _mapper.Map<Department>(request);
            await _applicationUnitOfWork.DepartmentRepository.UpdateAsync(UpdatedDepartment);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}