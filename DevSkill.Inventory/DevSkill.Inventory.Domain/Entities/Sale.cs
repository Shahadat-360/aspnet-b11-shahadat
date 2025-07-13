using DevSkill.Inventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class Sale:IEntity<string>
    {
        public string Id {  get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime SaleDate { get; set; }
        public DateTime SaleTime { get; set; }
        public decimal Total { get; set; }
        public decimal Due {  get; set; }
        public decimal Paid {  get; set; }
        public decimal Discount { get; set; }
        public decimal VatPercentage { get; set; }
        public  decimal NetAmmount { get; set; }
        public AccountType AccountType { get; set; } = AccountType.Cash;
        public int AccountId { get; set; }
        public string Note { get; set; }
        public string TermsAndConditions { get; set; }
        public SalesType SalesType { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Due;
        public IList<SaleItem> SaleItems { get; set; } = new List<SaleItem>();
    }
}
