using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Sales.Queries
{
    public class SalesByQuery:DataTables,IRequest<(IList<SalesIndexViewDto> data, int total, int totalDisplay)>
    {
        public SaleSearchDto? SearchItem { get; set; }
    }
}
