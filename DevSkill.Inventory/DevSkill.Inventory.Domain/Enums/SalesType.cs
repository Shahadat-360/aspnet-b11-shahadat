using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Enums
{
    public enum SalesType
    {
        [Display(Name ="MRP Sale")]
        MRP,
        [Display(Name ="WholeSale")]
        Wholesale
    }
}
