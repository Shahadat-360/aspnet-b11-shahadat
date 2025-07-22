using AutoMapper;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.BankAccounts.Queries
{
    public class BankAccountsByQueryHandler (IApplicationUnitOfWork applicationUnitOfWork,IMapper mapper)
        : IRequestHandler<BankAccountsByQuery, IList<BankAccountIndexDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<IList<BankAccountIndexDto>> Handle(BankAccountsByQuery request, CancellationToken cancellationToken)
        {
            var bankAccounts = await _applicationUnitOfWork.BankAccountRepository.GetAllAsync();
            var bankAccountsDto = _mapper.Map<IList<BankAccountIndexDto>>(bankAccounts);
            return bankAccountsDto;
        }
    }
}
