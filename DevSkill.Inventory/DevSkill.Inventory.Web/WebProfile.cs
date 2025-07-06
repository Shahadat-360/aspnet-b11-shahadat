using AutoMapper;
using DevSkill.Inventory.Application.Features.Categories.Commands;
using DevSkill.Inventory.Application.Features.Customers.Commands;
using DevSkill.Inventory.Application.Features.Products.Commands;
using DevSkill.Inventory.Application.Features.Units.Commands;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
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
            CreateMap<UnitAddCommand, Unit>().ReverseMap();
            CreateMap<Product, ProductViewDto>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
                .ForMember(d => d.UnitName, o => o.MapFrom(s => s.Unit.Name));
        }
    }
}
