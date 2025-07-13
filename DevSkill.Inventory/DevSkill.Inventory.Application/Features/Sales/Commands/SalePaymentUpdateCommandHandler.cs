using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Sales.Commands
{
    public class SalePaymentUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork) 
        : IRequestHandler<SalePaymentUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task Handle(SalePaymentUpdateCommand request, CancellationToken cancellationToken)
        {
            var sale = await _applicationUnitOfWork.SaleRepository.GetSaleByIdWithNavigationAsync(request.Id);
            sale.Due -= request.Paid;
            sale.Paid += request.Paid;
            sale.Customer.CurrentBalance += request.Paid;
            if (sale.Due == 0) sale.PaymentStatus = PaymentStatus.Paid;
            else if (sale.Due == sale.Total) sale.PaymentStatus = PaymentStatus.Due;
            else sale.PaymentStatus = PaymentStatus.Partial;

            //save
            await _applicationUnitOfWork.SaleRepository.UpdateAsync(sale);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
