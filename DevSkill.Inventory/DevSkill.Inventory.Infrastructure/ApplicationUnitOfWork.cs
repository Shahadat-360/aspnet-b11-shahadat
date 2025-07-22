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
        IProductRepository productRepository,ICustomerRepository customerRepository,
        ICategoryRepository categoryRepository,IUnitRepository unitRepository,
        ISaleRepository saleRepository,ICashAccountRepository cashAccountRepository,
        IBankAccountRepository bankAccountRepository,IMobileAccountRepository mobileAccountRepository,
        IBalanceTransferRepository balanceTransferRepository,IDepartmentRepository departmentRepository,
        IEmployeeRepository employeeRepository) 
        : UnitOfWork(applicationDbContext),IApplicationUnitOfWork
    {
        public IProductRepository ProductRepository { get; private set; } = productRepository;

        public ICustomerRepository CustomerRepository { get; private set; } = customerRepository;

        public ICategoryRepository CategoryRepository { get; set; } = categoryRepository;

        public IUnitRepository UnitRepository { get; private set; } = unitRepository;

        public ISaleRepository SaleRepository { get; private set; } = saleRepository;

        public ICashAccountRepository CashAccountRepository { get; private set; } = cashAccountRepository;

        public IBankAccountRepository BankAccountRepository { get; private set; } = bankAccountRepository;

        public IMobileAccountRepository MobileAccountRepository { get; private set; } = mobileAccountRepository;

        public IBalanceTransferRepository BalanceTransferRepository { get; private set; } = balanceTransferRepository;

        public IDepartmentRepository DepartmentRepository { get; private set; } = departmentRepository;

        public IEmployeeRepository EmployeeRepository { get; private set; } = employeeRepository;

        public async Task<(IList<BalanceTransferIndexDto>, int, int)> GetPagedBalanceTransfers(int pageIndex, int pageSize, string? order, BalanceTransferSearchDto? searchItem)
        {
            var storedProcedureName = "GetBalanceTransfers";
            var result = await _sqlUtility.QueryWithStoredProcedureAsync<BalanceTransferIndexDto>(storedProcedureName,
                new Dictionary<string, object>
                {
                    {"PageIndex",pageIndex },
                    {"PageSize",pageSize },
                    {"OrderBy",order },
                    {"TransferDateFrom",searchItem?.TransferDateFrom },
                    {"TransferDateTo",searchItem?.TransferDateTo },
                    {"SendingAccountType",searchItem?.SendingAccountType},
                    {"SendingAccount",string.IsNullOrEmpty(searchItem.SendingAccount)?null:searchItem.SendingAccount },
                    {"ReceivingAccountType",searchItem?.ReceivingAccountType},
                    {"ReceivingAccount",string.IsNullOrEmpty(searchItem.ReceivingAccount)?null:searchItem.ReceivingAccount },
                    {"MinTransferAmount",searchItem?.MinTransferAmount },
                    {"MaxTransferAmount",searchItem?.MaxTransferAmount},
                    {"Note",string.IsNullOrEmpty(searchItem?.Note)?null:searchItem.Note }
                },
                new Dictionary<string, Type> {
                    {"Total",typeof(int) },
                    {"TotalDisplay",typeof(int) }
                });
            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }

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

        public async Task<(IList<EmployeeDto> Employees, int total, int totalDisplay)>
            GetPagedEmployees(int pageIndex, int pageSize, string? order, EmployeeSearchDto? searchItem)
        {
            var storedProcedureName = "GetEmployees";
            var result = await _sqlUtility.QueryWithStoredProcedureAsync<EmployeeDto>(storedProcedureName,
                new Dictionary<string, object>
                {
                    {"PageIndex", pageIndex },
                    {"PageSize", pageSize },
                    {"OrderBy", order },
                    {"Id", string.IsNullOrEmpty(searchItem?.Id) ? null : searchItem.Id },
                    {"Name", string.IsNullOrEmpty(searchItem?.Name) ? null : searchItem.Name },
                    {"Mobile", string.IsNullOrEmpty(searchItem?.Mobile) ? null : searchItem.Mobile },
                    {"Email", string.IsNullOrEmpty(searchItem?.Email) ? null : searchItem.Email },
                    {"Address", string.IsNullOrEmpty(searchItem?.Address) ? null : searchItem.Address },
                    {"JoiningDateFrom", searchItem?.JoiningDateFrom },
                    {"JoiningDateTo", searchItem?.JoiningDateTo },
                    {"SalaryFrom", searchItem?.SalaryFrom },
                    {"SalaryTo", searchItem?.SalaryTo },
                    {"Status", searchItem?.Status }
                },
                new Dictionary<string, Type>
                {
                    {"Total", typeof(int) },
                    {"TotalDisplay", typeof(int) }
                });
            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }

        public async Task<(IList<ProductWithCategoryDto> products, int total, int totalDisplay)> 
            GetPagedProducts(int pageIndex, int pageSize, string? order, ProductSearchDto? searchItem)
        {
            var storedProcedureName = "GetProducts";
            var result = await _sqlUtility.QueryWithStoredProcedureAsync<ProductWithCategoryDto>(storedProcedureName,
                new Dictionary<string, object>
                {
                    {"PageIndex",pageIndex },
                    {"PageSize",pageSize },
                    {"OrderBy",order },
                    {"Id",string.IsNullOrEmpty(searchItem?.Id) ? null : searchItem.Id },
                    {"Name",string.IsNullOrEmpty(searchItem?.Name) ? null : searchItem.Name },
                    {"Category",string.IsNullOrEmpty(searchItem?.Category)?null:searchItem.Category },
                    {"MaxPurchasePrice",searchItem?.MaxPurchasePrice },
                    {"MinPurchasePrice",searchItem?.MinPurchasePrice },
                    {"MaxMRP",searchItem?.MaxMRP },
                    {"MinMRP",searchItem?.MinMRP },
                    {"MaxWholesalePrice",searchItem?.MaxWholesalePrice },
                    {"MinWholesalePrice",searchItem?.MinWholesalePrice }
                },
                new Dictionary<string, Type>
                {
                    {"Total",typeof(int) },
                    {"TotalDisplay",typeof(int) }
                });
            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }

        public async Task<(IList<SalesIndexViewDto> data, int total, int totalDisplay)> 
            GetPagedSales(int pageIndex, int pageSize, string? order, SaleSearchDto? searchItem)
        {
            var storedProcedureName = "GetSales";
            var result = await _sqlUtility.QueryWithStoredProcedureAsync<SalesIndexViewDto>(storedProcedureName,
                new Dictionary<string, object>
                {
                    {"PageIndex",pageIndex },
                    {"PageSize",pageSize },
                    {"OrderBy",order },
                    {"Id",string.IsNullOrEmpty(searchItem?.Id) ? null : searchItem.Id },
                    {"SalesDateFrom", searchItem?.SaleDateFrom },
                    {"SalesDateTo", searchItem?.SaleDateTo },
                    {"CustomerName",string.IsNullOrEmpty(searchItem?.CustomerName) ? null : searchItem.CustomerName },
                    {"CustomerPhone",string.IsNullOrEmpty(searchItem?.CustomerPhone) ? null : searchItem.CustomerPhone },
                    {"MinTotal",searchItem?.MinTotal },
                    {"MaxTotal",searchItem?.MaxTotal },
                    {"MinPaid",searchItem?.MinPaid },
                    {"MaxPaid",searchItem?.MaxPaid },
                    {"MinDue",searchItem?.MinDue },
                    {"MaxDue",searchItem?.MaxDue },
                    {"PaymentStatus",searchItem?.PaymentStatus }
                },
                new Dictionary<string, Type>
                {
                    {"Total",typeof(int) },
                    {"TotalDisplay",typeof(int) }
                });
            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }

        public async Task<(IList<UsersIndexDto> Users, int total, int totalDisplay)> 
            GetPagedUsers(int pageIndex, int pageSize, string? order, UserSearchDto? searchItem)
        {
            var storedProcedureName = "GetUsers";
            var result = await _sqlUtility.QueryWithStoredProcedureAsync<UsersIndexDto>(storedProcedureName,
                new Dictionary<string, object>
                {
                    {"PageIndex", pageIndex },
                    {"PageSize", pageSize },
                    {"OrderBy", order },
                    {"EmployeeName", string.IsNullOrEmpty(searchItem?.EmployeeName) ? null : searchItem.EmployeeName },
                    {"Email", string.IsNullOrEmpty(searchItem?.Email) ? null : searchItem.Email },
                    {"Mobile", string.IsNullOrEmpty(searchItem?.Mobile) ? null : searchItem.Mobile },
                    {"RoleName", string.IsNullOrEmpty(searchItem?.RoleName) ? null : searchItem.RoleName },
                    {"Status", searchItem?.Status }
                },
                new Dictionary<string, Type>
                {
                    {"Total", typeof(int) },
                    {"TotalDisplay", typeof(int) }
                });
            return (result.result, (int)result.outValues["Total"], (int)result.outValues["TotalDisplay"]);
        }
    }
}