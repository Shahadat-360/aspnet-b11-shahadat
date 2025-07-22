using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Categories.Commands
{
    public class CategoryDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) :
        IRequestHandler<CategoryDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork; 
        public async Task Handle(CategoryDeleteCommand request, CancellationToken cancellationToken)
        {
            var category = await _applicationUnitOfWork.CategoryRepository.GetByIdAsync(request.Id);
            await _applicationUnitOfWork.CategoryRepository.RemoveAsync(category);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
