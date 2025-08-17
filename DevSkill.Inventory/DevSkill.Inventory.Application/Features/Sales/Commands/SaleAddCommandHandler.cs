using AutoMapper;
using DevSkill.Inventory.Application.Features.Customers.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Sales.Commands
{
    public class SaleAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,
        IIdGenerator idGenerator,IMapper mapper) 
        : IRequestHandler<SaleAddCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly IMapper _mapper = mapper;
        public async Task Handle(SaleAddCommand request, CancellationToken cancellationToken)
        {
            var sale = _mapper.Map<Sale>(request);
            sale.Id = await _idGenerator.GenerateIdAsync("Inv-Dev");
            sale.SaleTime = DateTime.UtcNow;
            sale.PaymentStatus = sale.Paid == 0? PaymentStatus.Due:
                (sale.Paid < sale.Total)? PaymentStatus.Partial:PaymentStatus.Paid;
            
            //Get Customer
            var customer = await _applicationUnitOfWork.CustomerRepository.GetByIdAsync(sale.CustomerId);
            if(customer==null) 
                throw new InvalidOperationException($"Customer with ID {sale.CustomerId} not found");
            customer.CurrentBalance -= sale.Due;

            //sale product quantity update
            foreach (var item in sale.SaleItems)
            {
                var product = await _applicationUnitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                item.Id = await _idGenerator.GenerateIdAsync("Inv-Dev-Item");
                item.SaleId = sale.Id;
                if (product != null)
                {
                    int diff = product.Stock - item.Quantity;
                    if (diff < 0)
                        throw new InvalidOperationException(
                            $"Insufficient stock for product {item.ProductId}. Available: {product.Stock}, Required: {Math.Abs(diff)}");
                    else
                    {
                        product.Stock = diff;
                        await _applicationUnitOfWork.ProductRepository.UpdateAsync(product);
                    }
                }
                else
                {
                    throw new InvalidOperationException($"product: {product.Id} not found");
                }
            }

            await _applicationUnitOfWork.CustomerRepository.UpdateAsync(customer);
            await _applicationUnitOfWork.SaleRepository.AddAsync(sale);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
