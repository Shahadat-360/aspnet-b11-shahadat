using AutoMapper;
using DevSkill.Inventory.Application.Features.BalanceTransfers.Commands;
using DevSkill.Inventory.Application.Features.BankAccounts.Commands;
using DevSkill.Inventory.Application.Features.CashAccounts.Commands;
using DevSkill.Inventory.Application.Features.Categories.Commands;
using DevSkill.Inventory.Application.Features.Customers.Commands;
using DevSkill.Inventory.Application.Features.Departments.Commands;
using DevSkill.Inventory.Application.Features.Employees.Commands;
using DevSkill.Inventory.Application.Features.MobileAccounts.Commands;
using DevSkill.Inventory.Application.Features.MobileAccounts.Queries;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Application.Features.Sales.Commands;
using DevSkill.Inventory.Application.Features.Units.Commands;
using DevSkill.Inventory.Application.Features.UserRoles.Commands;
using DevSkill.Inventory.Application.Features.Users.Commands;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Web.Areas.Admin.Models;

namespace DevSkill.Inventory.Web
{
    public class WebProfile:Profile
    {
        public WebProfile()
        {
            CreateMap<ProductAddCommand, Product>().ReverseMap();
            CreateMap<ProductUpdateCommand, Product>()
                .ReverseMap()
                .ForMember(d=>d.CategoryName,o=>o.MapFrom(s=>s.Category.Name))
                .ForMember(d=>d.UnitName,o=>o.MapFrom(s=>s.Unit.Name));
            CreateMap<CustomerAddCommand, Customer>().ReverseMap();
            CreateMap<CustomerUpdateCommand, Customer>().ReverseMap();
            CreateMap<CategoryAddCommand, Category>().ReverseMap();
            CreateMap<Category, CategoryIndexDto>();
            CreateMap<CategoryUpdateCommand, Category>().ReverseMap();
            CreateMap<UnitAddCommand, Unit>().ReverseMap();
            CreateMap<Product, ProductViewDto>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
                .ForMember(d => d.UnitName, o => o.MapFrom(s => s.Unit.Name));
            CreateMap<SaleAddCommand, Sale>()
                .ForMember(d => d.SaleItems, o => o.MapFrom(s => s.SaleItems));
            CreateMap<SaleUpdateCommand, Sale>()
                .ForMember(d => d.SaleItems, o => o.Ignore());

            CreateMap<SaleItemDto, SaleItem>().ReverseMap();
            CreateMap<Sale, SaleItemDto>();
            CreateMap<Sale, SaleDto>().ReverseMap();
            CreateMap<SaleDto,SaleUpdateCommand>()
                .ForMember(d => d.SaleItems, o => o.MapFrom(s => s.SaleItems))
                .ForMember(d => d.CustomerName, o => o.MapFrom(s => s.Customer.CustomerName))
                .ReverseMap();
            CreateMap<SaleDto, SalePaymentUpdateCommand>()
                .ForMember(d => d.Paid, o => o.Ignore());
            CreateMap<BalanceTransferAddCommand, BalanceTransfer>();
            CreateMap<Customer,CustomerViewDto>()
                .ForMember(d=>d.Sales,o=>o.Ignore());
            CreateMap<BankAccount, BankAccountIndexDto>();
            CreateMap<BankAccountAddCommand, BankAccount>();
            CreateMap<BankAccountUpdateCommand, BankAccount>().ReverseMap();
            CreateMap<CashAccount, CashAccountIndexDto>();
            CreateMap<CashAccountAddCommand, CashAccount>();
            CreateMap<CashAccountUpdateCommand, CashAccount>().ReverseMap();
            CreateMap<MobileAccount, MobileAccountIndexDto>();
            CreateMap<MobileAccountAddCommand, MobileAccount>();
            CreateMap<MobileAccountUpdateCommand, MobileAccount>().ReverseMap();
            CreateMap<UnitUpdateCommand, Unit>().ReverseMap();
            CreateMap<Unit, UnitIndexDto>();
            CreateMap<ApplicationRole, UserRoleIndexDto>();
            CreateMap<UserRoleAddCommand, ApplicationRole>();
            CreateMap<UserRoleDto, UserRoleUpdateCommand>();
            CreateMap<UserRoleUpdateCommand,ApplicationRole>();
            CreateMap<ApplicationRole, UserRoleDto>();
            CreateMap<Department, DepartmentIndexDto>();
            CreateMap<DepartmentAddCommand, Department>();
            CreateMap<DepartmentUpdateCommand, Department>().ReverseMap();
            CreateMap<EmployeeAddCommand, Employee>();
            CreateMap<EmployeeUpdateCommand, Employee>().ReverseMap()
                .ForMember(d=>d.DepartmentName,o=>o.MapFrom(s=>s.Department.Name));
            CreateMap<UserDto, UserUpdateCommand>();
        }
    }
}
