using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using Unit = DevSkill.Inventory.Domain.Entities.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Units.Queries
{
    public class UnitSearchWithPaginationQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) :
        IRequestHandler<UnitSearchWithPaginationQuery, PaginatedResult<Unit>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<PaginatedResult<Unit>> Handle(UnitSearchWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.UnitRepository.SearchUnitWithPaginationAsync(request.term, request.page, request.pageSize);
        }
    }
}
