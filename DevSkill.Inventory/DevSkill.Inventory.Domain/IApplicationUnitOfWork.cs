using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain
{
    public interface IApplicationUnitOfWork:IUnitOfWork
    {
        public IProductRepository ProductRepository { get; }
        public ICustomerRepository CustomerRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IUnitRepository UnitRepository { get; }
        public ISaleRepository SaleRepository { get; }
        public ICashAccountRepository CashAccountRepository { get; }
        public IBankAccountRepository BankAccountRepository { get; }
        public IMobileAccountRepository MobileAccountRepository { get; }
        Task<(IList<Customer> customers, int total, int totalDisplay)> GetPagedCustomers
            (int pageIndex, int pageSize, string? order, CustomerSearchDto? searchItem);
        Task<(IList<ProductWithCategoryDto> products, int total, int totalDisplay)> GetPagedProducts
            (int pageIndex, int pageSize, string? order, ProductSearchDto? searchItem);
        Task<(IList<SalesIndexViewDto> data, int total, int totalDisplay)> GetPagedSales
             (int pageIndex, int pageSize, string? order, SaleSearchDto? searchItem);
    }
}
