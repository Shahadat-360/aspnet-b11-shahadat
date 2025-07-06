using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Products.Queries
{
    public class ProductSearchWithPaginationQuery:IRequest<PaginatedResult<Product>>
    {
        public string term = string.Empty;
        public int page = 1;
        public int pageSize = 5;
    }
}
