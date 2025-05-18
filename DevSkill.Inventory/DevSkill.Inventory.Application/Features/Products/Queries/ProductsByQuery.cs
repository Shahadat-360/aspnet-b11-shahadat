using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Products.Queries
{
    public class ProductsByQuery:DataTables,IRequest<(IList<Product>data,int total,int totalDisplay)>
    {
        public ProductSearchDto? SearchItem { get; set; }
    }
}
