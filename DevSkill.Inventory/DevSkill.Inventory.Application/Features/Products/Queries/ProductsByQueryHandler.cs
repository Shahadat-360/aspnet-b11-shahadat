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
    public class ProductsByQueryHandler(IApplicationUnitOfWork applicationUnitOfWork):
        IRequestHandler<ProductsByQuery,(IList<ProductWithCategoryDto> products, int total, int totalDisplay)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<(IList<ProductWithCategoryDto> products, int total, int totalDisplay)> Handle(ProductsByQuery request, CancellationToken cancellationToken)
        {
            string? order = request.FormatSortExpression("Id","Id","Id","Name","CategoryName", "PurchasePrice", "MRP", "WholesalePrice", "Stock", "LowStock", "DamageStock");
            return await _applicationUnitOfWork.GetPagedProducts(request.PageIndex, request.PageSize, order, request.SearchItem);
        }
    }
}
