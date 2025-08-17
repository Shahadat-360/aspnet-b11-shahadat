using AutoMapper;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Categories.Commands
{
    public class CategoryAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,IMapper mapper) 
        : IRequestHandler<CategoryAddCommand, Category>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<Category> Handle(CategoryAddCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request);
            category.Id = Guid.NewGuid();
            await _applicationUnitOfWork.CategoryRepository.AddAsync(category);
            await _applicationUnitOfWork.SaveAsync();
            return category;
        }
    }
}
