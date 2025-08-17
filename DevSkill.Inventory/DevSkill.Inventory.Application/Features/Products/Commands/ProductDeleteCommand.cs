using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Products.Commands
{
    public class ProductDeleteCommand:IRequest
    {
        public string Id { get; set; }
    }
}
