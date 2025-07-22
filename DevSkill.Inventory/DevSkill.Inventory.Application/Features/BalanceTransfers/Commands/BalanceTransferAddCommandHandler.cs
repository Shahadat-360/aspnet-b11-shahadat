using AutoMapper;
using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.BalanceTransfers.Commands
{
    public class BalanceTransferAddCommandHandler(IApplicationUnitOfWork applicationUnitOfWork
        ,IBalanceTransferService balanceTransferService,IMapper mapper,ITransactionService transactionService) 
        : IRequestHandler<BalanceTransferAddCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IBalanceTransferService _balanceTransferService = balanceTransferService;
        private readonly IMapper _mapper = mapper;
        private readonly ITransactionService _transactionService = transactionService;
        public async Task Handle(BalanceTransferAddCommand request, CancellationToken cancellationToken)
        {
            await _transactionService.BeginTransactionAsync();

            try
            {
                var SendingAccountCurrentBalance = await _balanceTransferService.GetAccountCurrentBalanceAsync(request.SendingAccountType, request.SendingAccountId);
                if(SendingAccountCurrentBalance<request.TransferAmount)
                    throw new InvalidOperationException($"Insufficient balance in sending account" +
                        $". Available: {SendingAccountCurrentBalance} Tk, Required: {request.TransferAmount} Tk");

            
                var balanceTransfer = _mapper.Map<BalanceTransfer>(request);
                await _applicationUnitOfWork.BalanceTransferRepository.AddAsync(balanceTransfer);
                await _balanceTransferService.UpdateAccountBalanceAsync(
                        request.SendingAccountType,
                        request.SendingAccountId,
                        SendingAccountCurrentBalance.Value - request.TransferAmount);

                var ReceivingAccountCurrentBalance = await _balanceTransferService.GetAccountCurrentBalanceAsync(request.ReceivingAccountType, request.ReceivingAccountId);
                await _balanceTransferService.UpdateAccountBalanceAsync(
                    request.ReceivingAccountType,
                    request.ReceivingAccountId,
                    ReceivingAccountCurrentBalance.Value + request.TransferAmount);

                await _applicationUnitOfWork.SaveAsync();
                await _transactionService.CommitTransactionAsync();
            }
            catch
            {
                await _transactionService.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
