using AutoMapper;
using DevSkill.Inventory.Domain;
using MediatR;
using Unit = DevSkill.Inventory.Domain.Entities.Unit;

namespace DevSkill.Inventory.Application.Features.Units.Commands
{
    public class UnitUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        : IRequestHandler<UnitUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task Handle(UnitUpdateCommand request, CancellationToken cancellationToken)
        {
            var updatedUnit = _mapper.Map<Unit>(request);
            await _applicationUnitOfWork.UnitRepository.UpdateAsync(updatedUnit);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}