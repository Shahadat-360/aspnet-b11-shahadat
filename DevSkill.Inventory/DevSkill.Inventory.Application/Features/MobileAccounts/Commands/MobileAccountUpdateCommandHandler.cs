using AutoMapper;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using MediatR;

namespace DevSkill.Inventory.Application.Features.MobileAccounts.Commands
{
    public class MobileAccountUpdateCommandHandler(IApplicationUnitOfWork applicationUnitOfWork, IMapper mapper)
    : IRequestHandler<MobileAccountUpdateCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IMapper _mapper = mapper;
        public async Task Handle(MobileAccountUpdateCommand request, CancellationToken cancellationToken)
        {
            var oldMobileAccount = await _applicationUnitOfWork.MobileAccountRepository.GetByIdAsNoTrackingAsync(request.Id);
            var newMobileAccount = _mapper.Map<MobileAccount>(request);
            newMobileAccount.CurrentBalance = oldMobileAccount.CurrentBalance + (request.OpeningBalance - oldMobileAccount.OpeningBalance);
            await _applicationUnitOfWork.MobileAccountRepository.UpdateAsync(newMobileAccount);
            await _applicationUnitOfWork.SaveAsync();
        }
    }
}