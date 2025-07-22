using DevSkill.Inventory.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Categories.Queries
{
    public class CategoriesByQuery:IRequest<IList<CategoryIndexDto>>
    {
    }
}
