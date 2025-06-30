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
    public class CustomerGetByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork) : IRequestHandler<CustomerGetByIdQuery, Customer>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task<Customer> Handle(CustomerGetByIdQuery request, CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork.CustomerRepository.GetByIdAsync(request.Id);
        }
    }
}
