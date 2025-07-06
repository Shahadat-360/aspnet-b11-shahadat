using AutoMapper;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Products.Commands
{
    public class ProductAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,IMapper mapper,
        IImageService imageService,IConfiguration configuration,IIdGenerator idGenerator) : IRequestHandler<ProductAddCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork=applicationUnitOfWork;
        private readonly IMapper _mapper=mapper;
        private readonly IImageService _imageService = imageService;
        private readonly IConfiguration _configuration = configuration;
        private readonly IIdGenerator _idGenerator = idGenerator;
        public async Task Handle(ProductAddCommand request, CancellationToken cancellationToken)
        {
            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                var UploadsFolder = _configuration["ImageUploadSettings:Product"];
                if (string.IsNullOrEmpty(UploadsFolder))
                {
                    throw new InvalidOperationException("Uploads folder path is not configured.");
                }
                request.ImageUrl = await _imageService.SaveImageAsync(request.ImageFile, UploadsFolder);
            }
            var product = _mapper.Map<Product>(request);
            product.Id= await _idGenerator.GenerateIdAsync("P-DEV");
            await _applicationUnitOfWork.ProductRepository.AddAsync(product);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
