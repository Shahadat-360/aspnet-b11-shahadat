using DevSkill.Inventory.Application.Services;
using Microsoft.EntityFrameworkCore.Storage;

namespace DevSkill.Inventory.Infrastructure.Services
{
    public class TransactionService(ApplicationDbContext applicationDbContext) : ITransactionService
    {
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
        private IDbContextTransaction _transaction;
        public async Task BeginTransactionAsync()
        {
            _transaction = await _applicationDbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
                await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }
    }
}
