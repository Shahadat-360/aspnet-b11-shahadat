using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DevSkill.Inventory.Infrastructure.Identity
{
    public class ApplicationUser:IdentityUser<Guid>
    {
        public bool IsOwner { get; set; } = false;
        public string? EmployeeId { get; set; }
        public Employee Employee { get; set; } 
        public Status Status { get; set; } = Status.Active;
    }
}
