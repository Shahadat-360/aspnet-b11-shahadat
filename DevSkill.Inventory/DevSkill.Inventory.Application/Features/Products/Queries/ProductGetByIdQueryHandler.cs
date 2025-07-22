using DevSkill.Inventory.Application.Services;
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
    public class ProductGetByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork,IImageService imageService)
        :IRequestHandler<ProductGetByIdQuery,Product>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IImageService _imageService = imageService;
        public async Task<Product> Handle(ProductGetByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _applicationUnitOfWork.ProductRepository.GetProductWithNavigationAsync(request.Id);
            if (product != null && !string.IsNullOrEmpty(product.ImageUrl))
            {
                product.ImageUrl = _imageService.GetPreSignedURL(product.ImageUrl);
            }
            else
            {
                throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
            }
            return product;
        }
    }
}
