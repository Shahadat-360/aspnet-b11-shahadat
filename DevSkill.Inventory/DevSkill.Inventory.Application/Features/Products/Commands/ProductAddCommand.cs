using DevSkill.Inventory.Domain.Entities;
using Unit = DevSkill.Inventory.Domain.Entities.Unit;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace DevSkill.Inventory.Application.Features.Products.Commands
{
    public class ProductAddCommand:IRequest
    {
        [Required(ErrorMessage ="Product Name is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Product Purchase Price is Required"),
            Range(1,int.MaxValue,ErrorMessage ="Product Purchase Pirce Must Be Greater Than 0")]
        public decimal? PurchasePrice { get; set; }
        [Required(ErrorMessage = "Product MRP is Required"),
            Range(1,int.MaxValue, ErrorMessage = "MRP Must Be Greater Than 0")]
        public decimal? MRP { get; set; }
        [Required(ErrorMessage = "Product Wholesale Price is Required"),
            Range(1,int.MaxValue,ErrorMessage ="Wholesale Price Must Be Greater Than 0")]
        public decimal? WholesalePrice { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int? Stock { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Low Stock cannot be negative.")]
        public int? LowStock { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Damage Stock cannot be negative.")]
        public int? DamageStock { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl { get; set; }
        [Required(ErrorMessage = "Unit is Required")]
        public Guid UnitId { get; set; }
        [Required(ErrorMessage = "Category is Required")]
        public Guid CategoryId { get; set; }
    }
}
