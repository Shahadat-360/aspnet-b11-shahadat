using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.BankAccounts.Queries
{
    public class BankAccountSearchWithPaginationQuery:IRequest<PaginatedResult<BankAccount>>
    {
        public string term { get; set; } = string.Empty;
        public int page { get; set; } = 1;
        public int pageSize { get; set; } = 5;
    }
}
