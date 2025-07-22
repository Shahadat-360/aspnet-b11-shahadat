using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Products.Commands
{
    public class ProductDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,IImageService imageService) 
        : IRequestHandler<ProductDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IImageService _imageService = imageService;
        public async Task Handle(ProductDeleteCommand request, CancellationToken cancellationToken)
        {
            var product = await _applicationUnitOfWork.ProductRepository.GetByIdAsync(request.Id);
            await _applicationUnitOfWork.ProductRepository.RemoveAsync(request.Id);
            await _applicationUnitOfWork.SaveAsync();
            if (product.ImageUrl != null)
            {
                await _imageService.DeleteImageAsync(product.ImageUrl);
            }
        }
    }
}
