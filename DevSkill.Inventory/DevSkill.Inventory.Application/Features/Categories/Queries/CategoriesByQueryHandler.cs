using AutoMapper;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;

namespace DevSkill.Inventory.Application.Features.Categories.Queries
{
    public class CategoriesByQueryHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        : IRequestHandler<CategoriesByQuery, IList<CategoryIndexDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<IList<CategoryIndexDto>> Handle(CategoriesByQuery request, CancellationToken cancellationToken)
        {
            var categories = await _applicationUnitOfWork.CategoryRepository.GetAllAsync();
            var categoriesDto = _mapper.Map<IList<CategoryIndexDto>>(categories);   
            return categoriesDto;
        }
    }
}