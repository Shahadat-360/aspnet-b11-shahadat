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

namespace DevSkill.Inventory.Application.Features.MobileAccounts.Queries
{
    public class MobileAccountsByQueryHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
        : IRequestHandler<MobileAccountsByQuery, IList<MobileAccountIndexDto>>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task<IList<MobileAccountIndexDto>> Handle(MobileAccountsByQuery request, CancellationToken cancellationToken)
        {
            var mobileAccounts = await _applicationUnitOfWork.MobileAccountRepository.GetAllAsync();
            var mobileAccountsDto = _mapper.Map<IList<MobileAccountIndexDto>>(mobileAccounts);
            return mobileAccountsDto;
        }
    }
}