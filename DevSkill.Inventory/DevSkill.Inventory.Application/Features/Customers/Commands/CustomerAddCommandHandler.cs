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
    public class CustomerAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,IMapper mapper,
        IIdGenerator idGenerator) : 
        IRequestHandler<CustomerAddCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IIdGenerator _idGenerator = idGenerator;
        public async Task Handle(CustomerAddCommand request, CancellationToken cancellationToken)
        {
            Customer customer = _mapper.Map<Customer>(request);
            customer.Id = await _idGenerator.GenerateIdAsync("C-DEV");
            await _applicationUnitOfWork.CustomerRepository.AddAsync(customer);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
