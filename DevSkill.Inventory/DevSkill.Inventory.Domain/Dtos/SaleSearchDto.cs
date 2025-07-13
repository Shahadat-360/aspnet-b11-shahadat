using DevSkill.Inventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class SaleSearchDto
    {
        public string? Id { get; set; }
        public DateOnly? SaleDateFrom { get; set; }
        public DateOnly? SaleDateTo { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public decimal? MinTotal {  get; set; }
        public decimal? MaxTotal { get; set; }
        public decimal? MinPaid { get; set; }
        public decimal? MaxPaid { get; set; }
        public decimal? MinDue { get; set; }
        public decimal? MaxDue { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
    }
}
