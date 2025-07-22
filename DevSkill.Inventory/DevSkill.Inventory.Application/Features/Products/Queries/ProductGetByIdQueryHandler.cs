using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Products.Queries
{
    public class ProductGetByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork,IImageService imageService,
        IConfiguration configuration)
        :IRequestHandler<ProductGetByIdQuery,Product>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IImageService _imageService = imageService;
        private readonly IConfiguration _configuration = configuration;
        public async Task<Product> Handle(ProductGetByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _applicationUnitOfWork.ProductRepository.GetProductWithNavigationAsync(request.Id);
            if (product != null && !string.IsNullOrEmpty(product.ImageUrl))
            {
                var folder = _configuration["ImageUploadSettings:Product"]!;
                var path = $"{folder}/{product.ImageUrl}";
                product.ImageUrl = _imageService.GetPreSignedURL(path);
            }
            else
            {
                throw new KeyNotFoundException($"Product with ID {request.Id} not found.");
            }
            return product;
        }
    }
}
