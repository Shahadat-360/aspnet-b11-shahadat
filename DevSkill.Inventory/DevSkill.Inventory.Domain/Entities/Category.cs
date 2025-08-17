using DevSkill.Inventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class Category:IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; } = Status.Active;
        public IList<Product> Products { get; set; } = new List<Product>();
    }
}
