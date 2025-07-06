using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Products.Queries
{
    public class ProductSearchWithPaginationQueryHandler(IApplicationUnitOfWork applicationUnitOfWork)
        : IRequestHandler<ProductSearchWithPaginationQuery, PaginatedResult<Product>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<PaginatedResult<Product>> Handle(ProductSearchWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.ProductRepository
                .SearchProductWithPaginationAsync(request.term,request.page,request.pageSize);
        }
    }
}
