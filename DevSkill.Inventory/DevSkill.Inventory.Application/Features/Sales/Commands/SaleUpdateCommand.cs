using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Features.Sales.Commands
{
    public class SaleUpdateCommand:IRequest
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Sale Date is required")]
        public DateTime SaleDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Customer is required")]
        public string CustomerId { get; set; }
        [Required(ErrorMessage = "Sale Type is required")]
        public string? CustomerName { get; set; }
        public SalesType SalesType { get; set; } = SalesType.MRP;
        [Required(ErrorMessage = "Product is required")]
        public string ProductId { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Vat Percentage must be non negative")]
        public decimal VatPercentage { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Discount must be non negative")]
        public decimal Discount { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Total must be non negative")]
        public decimal Total { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Paid must be non negative")]
        public decimal Paid { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Due must be non negative")]
        public decimal Due { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "NetAmount must be non negative")]
        public decimal NetAmmount { get; set; }
        [Required(ErrorMessage = "Account Type is required")]
        public AccountType AccountType { get; set; } = AccountType.Cash;
        [Required(ErrorMessage = "Account is required")]
        public int AccountId { get; set; }
        public string? AccountName { get; set; }
        public string? Note { get; set; }
        public string? TermsAndConditions { get; set; }
        public IList<SaleItemDto> SaleItems { get; set; } = new List<SaleItemDto>();
    }
}
