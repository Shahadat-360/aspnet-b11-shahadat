using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Products.Queries
{
    public class ProductGetByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork):IRequestHandler<ProductGetByIdQuery,Product>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork=applicationUnitOfWork;
        public async Task<Product> Handle(ProductGetByIdQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.ProductRepository.GetByIdAsync(request.Id);
        }
    }
}
