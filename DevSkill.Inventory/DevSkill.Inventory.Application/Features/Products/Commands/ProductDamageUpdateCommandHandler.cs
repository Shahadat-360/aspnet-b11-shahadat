using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Products.Commands
{
    public class ProductDamageUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) 
        : IRequestHandler<ProductDamageUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task Handle(ProductDamageUpdateCommand request, CancellationToken cancellationToken)
        {
            var product = await _applicationUnitOfWork.ProductRepository.GetByIdAsync(request.Id);
            if (product.Stock < request.Damage)
            {
                throw new InvalidOperationException("Insufficient stock to process damage update.");
            }
            product.Stock -= request.Damage;
            product.DamageStock += request.Damage;
            await _applicationUnitOfWork.ProductRepository.UpdateAsync(product);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
