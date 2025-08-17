using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Sales.Commands
{
    public class SaleDeleteCommand:IRequest
    {
        public string id {  get; set; }
    }
}
