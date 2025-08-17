using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Units.Commands
{
    public class UnitDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) :
        IRequestHandler<UnitDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork; 
        public async Task Handle(UnitDeleteCommand request, CancellationToken cancellationToken)
        {
            var unit = await _applicationUnitOfWork.UnitRepository.GetByIdAsync(request.Id);
            await _applicationUnitOfWork.UnitRepository.RemoveAsync(unit);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
