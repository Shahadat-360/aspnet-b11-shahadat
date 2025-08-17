using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Seeds
{
    public class AdminSeed(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;

        public async Task SeedAdminAsync()
        {
            var username = _configuration["AdminSettings:Username"];
            var email = _configuration["AdminSettings:Email"];
            var password = _configuration["AdminSettings:Password"];

            var adminUser = await _userManager.FindByEmailAsync(email);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = username,
                    Email = email,
                    EmailConfirmed = true,
                    IsOwner = true
                };
                var result = await _userManager.CreateAsync(adminUser, password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}