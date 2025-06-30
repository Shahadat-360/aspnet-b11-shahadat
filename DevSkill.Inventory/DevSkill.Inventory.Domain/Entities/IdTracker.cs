using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class IdTracker
    {
        public string Prefix { get; set; }
        public int LastUsedNumber { get; set; }
    }
}
