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
        IProductRepository productRepository) 
        : UnitOfWork(applicationDbContext),IApplicationUnitOfWork
    {
        public IProductRepository ProductRepository { get; private set; } = productRepository;

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