using DevSkill.Inventory.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Seeds
{
    public static class RoleSeed
    {
        public static ApplicationRole[] GetRoles ()
        {
            return 
            [
                new ApplicationRole{
                    Id = new Guid("79149158-E28B-4EC3-B110-98CD62CB58BF"),
                    Name="Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = new DateTime(2025,7,10,1,1,1).ToString(),
                    Company = "DevSkill",
                    Status = Domain.Enums.Status.Active
                }
            ];
        }
    }
}
