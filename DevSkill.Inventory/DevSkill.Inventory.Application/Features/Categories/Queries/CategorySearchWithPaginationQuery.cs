using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Categories.Queries
{
    public class CategorySearchWithPaginationQuery : IRequest<PaginatedResult<Category>>
    {
        public string term=string.Empty;
        public int page = 1; 
        public int pageSize = 5;
    }
}
