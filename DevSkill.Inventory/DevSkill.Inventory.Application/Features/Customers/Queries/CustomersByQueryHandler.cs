using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Customers.Queries
{
    public class CustomersByQueryHandler(IApplicationUnitOfWork applicationUnitOfWork):IRequestHandler<CustomersByQuery, (IList<Customer> customers, int total, int totalDisplay)>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<(IList<Customer> customers, int total, int totalDisplay)> Handle(CustomersByQuery request, CancellationToken cancellationToken)
        {
            string? order = request.FormatSortExpression("ID","Id","CustomerName", "CompanyName", "Mobile", "Address", "Email",  "CurrentBalance","Status");
            return await _applicationUnitOfWork.GetPagedCustomers(request.PageIndex, request.PageSize, order, request.SearchItem);
        }
    }
}
