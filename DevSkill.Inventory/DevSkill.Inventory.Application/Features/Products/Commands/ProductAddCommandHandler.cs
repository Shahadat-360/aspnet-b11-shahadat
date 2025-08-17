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
        IImageService imageService,IConfiguration configuration,IIdGenerator idGenerator,ISqsService sqsService
        ) : IRequestHandler<ProductAddCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork=applicationUnitOfWork;
        private readonly IMapper _mapper=mapper;
        private readonly IImageService _imageService = imageService;
        private readonly IConfiguration _configuration = configuration;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ISqsService _sqsService = sqsService;
        public async Task Handle(ProductAddCommand request, CancellationToken cancellationToken)
        {
            if (request.ImageFile != null)
            {
                var folder = _configuration["ImageUploadSettings:Product"]!;
                var key = await _imageService.SaveImageAsync(request.ImageFile, folder);
                request.ImageUrl = key;
                await _sqsService.SendKeyAsync(key);
            }
            var product = _mapper.Map<Product>(request);
            product.Id= await _idGenerator.GenerateIdAsync("P-DEV");
            await _applicationUnitOfWork.ProductRepository.AddAsync(product);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
