using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class Product:IEntity<string>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal MRP { get; set; }
        public decimal WholesalePrice { get; set; }
        public int Stock { get; set; }
        public int LowStock { get; set; }
        public int DamageStock { get; set; }
        public string ImageUrl { get; set; }
        public Guid UnitId { get; set; }
        public Unit Unit { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public IList<Sale> Sales { get; set; } = new List<Sale>();
    }
}
