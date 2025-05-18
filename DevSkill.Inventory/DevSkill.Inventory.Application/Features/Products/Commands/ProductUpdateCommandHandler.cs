using AutoMapper;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Products.Commands
{
    public class ProductUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,IMapper mapper) : IRequestHandler<ProductUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork=applicationUnitOfWork;
        private readonly IMapper _mapper=mapper;
        public async Task Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
        {
            var updatedProduct = _mapper.Map<Product>(request);
            updatedProduct.UpdatedAt = DateTime.UtcNow;
            await _applicationUnitOfWork.ProductRepository.UpdateAsync(updatedProduct);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
