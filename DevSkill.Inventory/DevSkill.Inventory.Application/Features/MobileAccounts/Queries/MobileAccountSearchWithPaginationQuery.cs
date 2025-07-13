using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.MobileAccounts.Queries
{
    public class MobileAccountSearchWithPaginationQuery:IRequest<PaginatedResult<MobileAccount>>
    {
        public string term {  get; set; }=string.Empty;
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}
