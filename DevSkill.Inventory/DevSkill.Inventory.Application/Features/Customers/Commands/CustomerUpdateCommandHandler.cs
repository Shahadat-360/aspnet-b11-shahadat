using AutoMapper;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Customers.Commands
{
    public class CustomerUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,
        IMapper mapper) : IRequestHandler<CustomerUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
        {
            var customer = _mapper.Map<Customer>(request);
            await _applicationUnitOfWork.CustomerRepository.UpdateAsync(customer);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
