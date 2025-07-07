using DevSkill.Inventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class Sales:IEntity<string>
    {
        public string Id {  get; set; }
        public string Name { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Guid ProductId {  get; set; }
        public Product Product { get; set; }
        public int Quantity {  get; set; }
        public DateOnly DateOnly { get; set; }
        public TimeOnly TimeOnly { get; set; }
        public decimal Total { get; set; }
        public decimal Due {  get; set; }
        public decimal Paid {  get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Due;
        public decimal UnitPrice { get; set; }


    }
}
