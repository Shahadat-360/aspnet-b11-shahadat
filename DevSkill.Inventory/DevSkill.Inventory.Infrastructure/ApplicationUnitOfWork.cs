using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Repositories;
using DevSkill.Inventory.Domain.Utilities;
using DevSkill.Inventory.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure
{
    public class ApplicationUnitOfWork(ApplicationDbContext applicationDbContext, 
        IProductRepository productRepository,ICustomerRepository customerRepository) 
        : UnitOfWork(applicationDbContext),IApplicationUnitOfWork
    {
        public IProductRepository ProductRepository { get; private set; } = productRepository;

        public ICustomerRepository CustomerRepository { get; private set; } = customerRepository;

        public async Task<(IList<Customer> customers, int total, int totalDisplay)> GetPagedCustomers
            (int pageIndex, int pageSize, string? order, CustomerSearchDto? searchItem)
        {
            var storedProcedureName = "GetCustomers";
            var result = await _sqlUtility.QueryWithStoredProcedureAsync<Customer>(storedProcedureName,
                new Dictionary<string, object>
                {
                    {"PageIndex",pageIndex },
                    {"PageSize",pageSize },
                    {"OrderBy",order },
                    {"CustomerName",string.IsNullOrEmpty(searchItem.CustomerName)?null:searchItem.CustomerName },
                    {"CompanyName",string.IsNullOrEmpty(searchItem.CustomerName)?null:searchItem.CompanyName },
                    {"Email",string.IsNullOrEmpty(searchItem.Email)?null:searchItem.Email },
                    {"Mobile",string.IsNullOrEmpty(searchItem.Mobile)?null:searchItem.Mobile },
                    {"Address",string.IsNullOrEmpty(searchItem.Address)?null:searchItem.Address },
                    {"MinCurrentBalance",searchItem?.MinCurrentBalance },
                    {"MaxCurrentBalance",searchItem?.MaxCurrentBalance },
                    {"Status",searchItem?.Status }
                },
                new Dictionary<string, Type>
                {
                    {"Total",typeof(int) },
                    {"TotalDisplay",typeof(int) }
                });
            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }

        public async Task<(IList<Product> products, int total, int totalDisplay)> 
            GetPagedProducts(int pageIndex, int pageSize, string? order, ProductSearchDto? searchItem)
        {
            var storedProcedureName = "GetProducts";
            var result = await _sqlUtility.QueryWithStoredProcedureAsync<Product>(storedProcedureName,
                new Dictionary<string, object>
                {
                    {"PageIndex",pageIndex },
                    {"PageSize",pageSize },
                    {"OrderBy",order },
                    {"Name",string.IsNullOrEmpty(searchItem?.Name) ? null : searchItem.Name },
                    {"CategoryId",searchItem?.CategoryId },
                    {"MinPrice",searchItem?.MinPrice },
                    {"MaxPrice",searchItem?.MaxPrice }
                },
                new Dictionary<string, Type>
                {
                    {"Total",typeof(int) },
                    {"TotalDisplay",typeof(int) }
                });
            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);

        }
    }
}