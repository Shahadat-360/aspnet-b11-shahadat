using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class ProductWithCategoryDto
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal MRP { get; set; }
        public decimal WholesalePrice { get; set; }
        public int Stock { get; set; }
        public int LowStock { get; set; }
        public int DamageStock { get; set; }
    }
}