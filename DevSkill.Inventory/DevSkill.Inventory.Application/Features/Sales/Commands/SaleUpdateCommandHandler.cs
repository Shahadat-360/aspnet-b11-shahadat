using AutoMapper;
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
    public class SaleUpdateCommandHandler (IApplicationUnitOfWork applicationUnitOfWork,
        IMapper mapper) : IRequestHandler<SaleUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task Handle(SaleUpdateCommand request, CancellationToken cancellationToken)
        {
            var sale = await _applicationUnitOfWork.SaleRepository.GetSaleByIdWithNavigationAsync(request.Id);
            var NewCustomer = await _applicationUnitOfWork.CustomerRepository.GetByIdAsync(request.CustomerId);
            var OldCustomer = sale.Customer;

            if (OldCustomer.Id == NewCustomer.Id)
            {
                NewCustomer.CurrentBalance += (sale.Due - request.Due);
            }
            else
            {
                OldCustomer.CurrentBalance -= sale.Due;
                await _applicationUnitOfWork.CustomerRepository.UpdateAsync(OldCustomer);
                NewCustomer.CurrentBalance -= request.Due;
            }

            _mapper.Map(request, sale);
            sale.PaymentStatus = sale.Paid == 0 ? PaymentStatus.Due :
                (sale.Paid < sale.Total) ? PaymentStatus.Partial : PaymentStatus.Paid;

            //SaleItems Update 
            sale = await _applicationUnitOfWork.SaleRepository.UpdateSaleWithItemsAsync(sale, request.SaleItems);

            //update customer current balance
            await _applicationUnitOfWork.CustomerRepository.UpdateAsync(NewCustomer);

            //update sale
            await _applicationUnitOfWork.SaleRepository.UpdateAsync(sale);

            //save all changes
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
