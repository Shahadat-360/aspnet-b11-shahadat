using DevSkill.Inventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class Customer : IEntity<string>
    {
        public string Id {get; set; }
        public string CustomerName { get; set; }
        public string CompanyName { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int OpeningBalance { get; set; }
        public decimal CurrentBalance { get; set; }
        public Status Status { get; set; }
    }
}
