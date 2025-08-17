using DevSkill.Inventory.Application.Services;
using DevSkill.Inventory.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.BalanceTransfers.Commands
{
    public class BalanceTransferDeleteCommandHandler(IApplicationUnitOfWork applicationUnitOfWork,
        IBalanceTransferService balanceTransferService,ITransactionService transactionService) 
        : IRequestHandler<BalanceTransferDeleteCommand>
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork = applicationUnitOfWork;
        private readonly IBalanceTransferService _balanceTransferService = balanceTransferService;
        private readonly ITransactionService _transactionService = transactionService;
        public async Task Handle(BalanceTransferDeleteCommand request, CancellationToken cancellationToken)
        {
            await _transactionService.BeginTransactionAsync();
            try
            {
                var balanceTransfer = await _applicationUnitOfWork.BalanceTransferRepository.GetByIdAsync(request.Id);
                if (balanceTransfer == null)
                    throw new InvalidOperationException($"Balance transfer with ID {request.Id} not found.");


                var ReceivingAccountCurrentBalance = await _balanceTransferService
                    .GetAccountCurrentBalanceAsync(balanceTransfer.ReceivingAccountType,balanceTransfer.ReceivingAccountId);

                if (ReceivingAccountCurrentBalance < balanceTransfer.TransferAmount)
                    throw new InvalidOperationException($"Cannot reverse transfer. Insufficient balance in receiving account" +
                        $". Available: {ReceivingAccountCurrentBalance} Tk, Required: {balanceTransfer.TransferAmount } Tk");


                await _balanceTransferService.UpdateAccountBalanceAsync(
                    balanceTransfer.ReceivingAccountType,
                    balanceTransfer.ReceivingAccountId,
                    ReceivingAccountCurrentBalance.Value - balanceTransfer.TransferAmount);

                var SendingAccountCurrentBalance = await _balanceTransferService
                    .GetAccountCurrentBalanceAsync(balanceTransfer.SendingAccountType, balanceTransfer.SendingAccountId);
                await _balanceTransferService.UpdateAccountBalanceAsync(
                    balanceTransfer.SendingAccountType,
                    balanceTransfer.SendingAccountId,
                    SendingAccountCurrentBalance.Value + balanceTransfer.TransferAmount);

                await _applicationUnitOfWork.BalanceTransferRepository.RemoveAsync(balanceTransfer);
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
