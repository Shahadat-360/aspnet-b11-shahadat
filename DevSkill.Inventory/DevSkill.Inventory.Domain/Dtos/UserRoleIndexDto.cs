using DevSkill.Inventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class UserRoleIndexDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public Status Status { get; set; }
    }
}
