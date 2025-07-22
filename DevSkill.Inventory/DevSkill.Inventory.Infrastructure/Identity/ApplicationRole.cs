using DevSkill.Inventory.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Identity
{
    public class ApplicationRole: IdentityRole<Guid>
    {
        public string Company { get; set; } = "DevSkill";
        public Status Status { get; set; } = Status.Active;
    }
}
