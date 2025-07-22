using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Identity
{
    public class ApplicationRoleManager(IRoleStore<ApplicationRole> store,
        IEnumerable<IRoleValidator<ApplicationRole>> roleValidators,
        ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
        ILogger<RoleManager<ApplicationRole>> logger) 
        : RoleManager<ApplicationRole>(store, roleValidators, keyNormalizer, errors, logger)
    {
    }
}
