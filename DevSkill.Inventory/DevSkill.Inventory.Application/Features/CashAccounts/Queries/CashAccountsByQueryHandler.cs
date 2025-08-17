using AutoMapper;
using DevSkill.Inventory.Application.Features.BankAccounts.Queries;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.CashAccounts.Queries
{
    public class CashAccountsByQueryHandler (IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        : IRequestHandler<CashAccountsByQuery, IList<CashAccountIndexDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<IList<CashAccountIndexDto>> Handle(CashAccountsByQuery request, CancellationToken cancellationToken)
        {
            var cashAccounts = await _applicationUnitOfWork.CashAccountRepository.GetAllAsync();
            var cashAccountsDto = _mapper.Map<IList<CashAccountIndexDto>>(cashAccounts);
            return cashAccountsDto;
        }
    }
}