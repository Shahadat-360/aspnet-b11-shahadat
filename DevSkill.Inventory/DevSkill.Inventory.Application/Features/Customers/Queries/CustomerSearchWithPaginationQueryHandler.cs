using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Customers.Queries
{
    public class CustomerSearchWithPaginationQueryHandler (IApplicationUnitOfWork applicationUnitOfWork)
        : IRequestHandler<CustomerSearchWithPaginationQuery, PaginatedResult<Customer>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<PaginatedResult<Customer>> Handle(CustomerSearchWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.CustomerRepository
                .SearchCustomerWithPaginationAsync(request.term, request.page, request.pageSize);
        }
    }
}
