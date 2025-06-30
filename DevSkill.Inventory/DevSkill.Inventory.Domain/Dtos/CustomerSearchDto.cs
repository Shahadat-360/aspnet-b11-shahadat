using DevSkill.Inventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class CustomerSearchDto
    {
        public string? CustomerName { get; set; }
        public string? CompanyName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? Address { get; set; }
        public decimal? MaxCurrentBalance { get; set; }
        public decimal? MinCurrentBalance { get; set; }
        public Status? Status { get; set; }
    }
}
