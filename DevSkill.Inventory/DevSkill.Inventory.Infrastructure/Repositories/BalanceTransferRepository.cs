using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class BalanceTransferRepository(ApplicationDbContext applicationDbContext)
        :Repository<BalanceTransfer,Guid>(applicationDbContext),IBalanceTransferRepository
    {

    }
}
