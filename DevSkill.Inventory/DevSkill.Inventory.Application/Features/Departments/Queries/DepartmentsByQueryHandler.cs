using AutoMapper;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Departments.Queries
{
    public class DepartmentsByQueryHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        : IRequestHandler<DepartmentsByQuery, IList<DepartmentIndexDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<IList<DepartmentIndexDto>> Handle(DepartmentsByQuery request, CancellationToken cancellationToken)
        {
            var departments = await _applicationUnitOfWork.DepartmentRepository.GetAllAsync();
            var departmentDto = _mapper.Map<IList<DepartmentIndexDto>>(departments);   
            return departmentDto;
        }
    }
}