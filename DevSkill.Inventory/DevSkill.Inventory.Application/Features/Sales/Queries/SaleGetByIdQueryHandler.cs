using AutoMapper;
using DevSkill.Inventory.Application.Features.BankAccounts.Queries;
using DevSkill.Inventory.Application.Features.CashAccounts.Queries;
using DevSkill.Inventory.Application.Features.MobileAccounts.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Sales.Queries
{
    public class SaleGetByIdQueryHandler(IApplicationUnitOfWork applicationUnitOfWork,IMapper mapper,
        IMediator mediator) : IRequestHandler<SaleGetByIdQuery, SaleDto>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IMediator _mediator = mediator;
        public async Task<SaleDto> Handle(SaleGetByIdQuery request, CancellationToken cancellationToken)
        {
            var sale = await _applicationUnitOfWork.SaleRepository.GetSaleByIdWithNavigationAsync(request.Id);
            var saleDto = _mapper.Map<SaleDto>(sale);
            saleDto.AccountName = saleDto.AccountType switch
            {
                AccountType.Cash => (await _mediator.Send(new CashAccountGetByIdQuery { Id = saleDto.AccountId }))?.AccountName,
                AccountType.Bank => (await _mediator.Send(new BankAccountGetByIdQuery { Id = saleDto.AccountId }))?.AccountName,
                AccountType.Mobile => (await _mediator.Send(new MobileAccountGetByIdQuery { Id = saleDto.AccountId }))?.AccountName,
                _ => null
            };
            return saleDto;
        }
    }
}
