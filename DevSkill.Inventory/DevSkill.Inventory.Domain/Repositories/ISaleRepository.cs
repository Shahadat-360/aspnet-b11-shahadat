using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Repositories
{
    public interface ISaleRepository : IRepository<Sale, string>
    {
        Task<Sale> GetSaleByIdWithNavigationAsync(string id);
        Task<Sale> UpdateSaleWithItemsAsync(Sale sale, IList<SaleItemDto> newSaleItems);
        void ProductQuantityRestored(Sale sale);
    }
}
