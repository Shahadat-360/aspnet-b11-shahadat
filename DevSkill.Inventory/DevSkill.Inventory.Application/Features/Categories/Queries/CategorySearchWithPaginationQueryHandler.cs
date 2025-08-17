using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Categories.Queries
{
    public class CategorySearchWithPaginationQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) :
        IRequestHandler<CategorySearchWithPaginationQuery, PaginatedResult<Category>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<PaginatedResult<Category>> Handle(CategorySearchWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.CategoryRepository.SearcCategoryhWithPaginationAsync(request.term, request.page, request.pageSize);
        }
    }
}
