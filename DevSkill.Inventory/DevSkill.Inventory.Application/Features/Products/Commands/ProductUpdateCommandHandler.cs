using AutoMapper;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Products.Commands
{
    public class ProductUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,IMapper mapper,
        IConfiguration configuration,IImageService imageService) : IRequestHandler<ProductUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IConfiguration _configuration = configuration;
        private readonly IImageService _imageService = imageService;
        public async Task Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
        {
            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                    var UploadsFolder = _configuration["ImageUploadSettings:Product"];
                    if (string.IsNullOrEmpty(UploadsFolder))
                    {
                        throw new InvalidOperationException("Uploads folder path is not configured.");
                    }
                    if (!string.IsNullOrEmpty(request.ImageUrl))
                        await _imageService.DeleteImageAsync(request.ImageUrl);
                    request.ImageUrl = await _imageService.SaveImageAsync(request.ImageFile, UploadsFolder);
            }
            if(request.ImageBackup!= null && request.ImageBackup != request.ImageUrl)
            {
                await _imageService.DeleteImageAsync(request.ImageBackup);
            }
            var updatedProduct = _mapper.Map<Product>(request);
            await _applicationUnitOfWork.ProductRepository.UpdateAsync(updatedProduct);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
