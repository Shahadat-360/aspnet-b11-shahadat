using DevSkill.Inventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class SalesIndexViewDto
    {
        public string Id { get;set; }
        public DateTime SaleDate { get; set; }
        public string CustomerName{ get; set; }
        public string CustomerPhone { get; set; }
        public decimal Total {  get; set; }
        public decimal Paid { get; set; }
        public decimal Due { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
