using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Sales.Commands
{
    internal class SaleDeleteCommandHandler (IApplicationUnitOfWork applicationUnitOfWork)
        : IRequestHandler<SaleDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        public async Task Handle(SaleDeleteCommand request, CancellationToken cancellationToken)
        {
            var sale = await _applicationUnitOfWork.SaleRepository.GetSaleByIdWithNavigationAsync(request.id);
            var customer = await _applicationUnitOfWork.CustomerRepository.GetByIdAsync(sale.CustomerId);
            customer.CurrentBalance += sale.Due;

            _applicationUnitOfWork.SaleRepository.ProductQuantityRestored(sale);
            await _applicationUnitOfWork.CustomerRepository.UpdateAsync(customer);
            await _applicationUnitOfWork.SaleRepository.RemoveAsync(sale);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}
