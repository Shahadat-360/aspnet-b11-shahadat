using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Products.Commands
{
    public class ProductStockUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork)
        : IRequestHandler<ProductStockUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task Handle(ProductStockUpdateCommand request, CancellationToken cancellationToken)
        {
            var product = await _applicationUnitOfWork.ProductRepository.GetByIdAsync(request.Id);
            product.Stock+= request.Stock;
            await _applicationUnitOfWork.ProductRepository.UpdateAsync(product);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
