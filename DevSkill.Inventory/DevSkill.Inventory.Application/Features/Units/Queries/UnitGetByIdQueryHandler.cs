using DevSkill.Inventory.Application.Features.Units.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using Unit = DevSkill.Inventory.Domain.Entities.Unit;

namespace DevSkill.Inventory.Application.Features.Units.Queries
{
    public class UnitGetByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) 
        : IRequestHandler<UnitGetByIdQuery, Unit>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<Unit> Handle(UnitGetByIdQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.UnitRepository.GetByIdAsync(request.Id);
        }
    }
}