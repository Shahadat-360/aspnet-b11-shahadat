using AutoMapper;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Categories.Commands
{
    public class CategoryUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        : IRequestHandler<CategoryUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
        {
            var UpdatedCategory = _mapper.Map<Category>(request);
            await _applicationUnitOfWork.CategoryRepository.UpdateAsync(UpdatedCategory);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}