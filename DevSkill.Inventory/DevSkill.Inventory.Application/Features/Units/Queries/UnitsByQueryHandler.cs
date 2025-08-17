using AutoMapper;
using DevSkill.Inventory.Application.Features.Units.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Units.Queries
{
    public class UnitsByQueryHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        : IRequestHandler<UnitsByQuery, IList<UnitIndexDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<IList<UnitIndexDto>> Handle(UnitsByQuery request, CancellationToken cancellationToken)
        {
            var Units = await _applicationUnitOfWork.UnitRepository.GetAllAsync();
            var UnitsDto = _mapper.Map<IList<UnitIndexDto>>(Units);
            return UnitsDto;
        }
    }
}