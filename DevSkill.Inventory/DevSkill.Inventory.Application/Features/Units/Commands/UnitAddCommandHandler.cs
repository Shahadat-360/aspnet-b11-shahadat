using AutoMapper;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using Unit = DevSkill.Inventory.Domain.Entities.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Units.Commands
{
    public class UnitAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,IMapper mapper) 
        : IRequestHandler<UnitAddCommand,Unit>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<Unit> Handle(UnitAddCommand request, CancellationToken cancellationToken)
        {
            var unit = _mapper.Map<Unit>(request);
            unit.Id = Guid.NewGuid();
            await _applicationUnitOfWork.UnitRepository.AddAsync(unit);
            await _applicationUnitOfWork.SaveAsync();
            return unit;
        }
    }
}
