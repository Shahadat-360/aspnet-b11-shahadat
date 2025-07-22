using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Categories.Queries
{
    public class CategoryGetByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) 
        : IRequestHandler<CategoryGetByIdQuery, Category>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<Category> Handle(CategoryGetByIdQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.CategoryRepository.GetByIdAsync(request.Id);
        }
    }
}