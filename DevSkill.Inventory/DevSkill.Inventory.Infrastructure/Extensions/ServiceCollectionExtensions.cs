using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DevSkill.Inventory.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<ApplicationUserManager>()
                .AddRoleManager<ApplicationRoleManager>()
                .AddSignInManager<ApplicationSignInManager>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                //options.Lockout.MaxFailedAccessAttempts = 5;
                //options.Lockout.AllowedForNewUsers = false;

                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });
        }

        public static void AddPolicy(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                //Customer 
                options.AddPolicy(Permissions.CustomerAdd, policy =>
                    policy.RequireClaim(PermissionTypes.Customer, Permissions.CustomerAdd));

                options.AddPolicy(Permissions.CustomerUpdate, policy =>
                    policy.RequireClaim(PermissionTypes.Customer, Permissions.CustomerUpdate));

                options.AddPolicy(Permissions.CustomerDelete, policy =>
                    policy.RequireClaim(PermissionTypes.Customer, Permissions.CustomerDelete));

                options.AddPolicy(Permissions.CustomerPage, policy =>
                    policy.RequireClaim(PermissionTypes.Customer, Permissions.CustomerPage));

                //Product
                options.AddPolicy(Permissions.ProductAdd, policy =>
                    policy.RequireClaim(PermissionTypes.Product, Permissions.ProductAdd));

                options.AddPolicy(Permissions.ProductUpdate, policy =>
                    policy.RequireClaim(PermissionTypes.Product, Permissions.ProductUpdate));

                options.AddPolicy(Permissions.ProductDelete, policy =>
                    policy.RequireClaim(PermissionTypes.Product, Permissions.ProductDelete));

                options.AddPolicy(Permissions.ProductPage, policy =>
                    policy.RequireClaim(PermissionTypes.Product, Permissions.ProductPage));

                //Sale 
                options.AddPolicy(Permissions.SaleAdd, policy =>
                    policy.RequireClaim(PermissionTypes.Sale, Permissions.SaleAdd));

                options.AddPolicy(Permissions.SaleUpdate, policy =>
                    policy.RequireClaim(PermissionTypes.Sale, Permissions.SaleUpdate));

                options.AddPolicy(Permissions.SaleDelete, policy =>
                    policy.RequireClaim(PermissionTypes.Sale, Permissions.SaleDelete));

                options.AddPolicy(Permissions.SalePage, policy =>
                    policy.RequireClaim(PermissionTypes.Sale, Permissions.SalePage));

                //BalanceTransfer 
                options.AddPolicy(Permissions.BalanceTransferAdd, policy =>
                    policy.RequireClaim(PermissionTypes.BalanceTransfer, Permissions.BalanceTransferAdd));

                options.AddPolicy(Permissions.BalanceTransferDelete, policy =>
                    policy.RequireClaim(PermissionTypes.BalanceTransfer, Permissions.BalanceTransferDelete));

                options.AddPolicy(Permissions.BalanceTransferPage, policy =>
                    policy.RequireClaim(PermissionTypes.BalanceTransfer, Permissions.BalanceTransferPage));

                // Bank Account
                options.AddPolicy(Permissions.BankAccountAdd, policy =>
                    policy.RequireClaim(PermissionTypes.BankAccount, Permissions.BankAccountAdd));

                options.AddPolicy(Permissions.BankAccountUpdate, policy =>
                    policy.RequireClaim(PermissionTypes.BankAccount, Permissions.BankAccountUpdate));

                options.AddPolicy(Permissions.BankAccountDelete, policy =>
                    policy.RequireClaim(PermissionTypes.BankAccount, Permissions.BankAccountDelete));

                options.AddPolicy(Permissions.BankAccountPage, policy =>
                    policy.RequireClaim(PermissionTypes.BankAccount, Permissions.BankAccountPage));

                // Cash Account 
                options.AddPolicy(Permissions.CashAccountAdd, policy =>
                    policy.RequireClaim(PermissionTypes.CashAccount, Permissions.CashAccountAdd));

                options.AddPolicy(Permissions.CashAccountUpdate, policy =>
                    policy.RequireClaim(PermissionTypes.CashAccount, Permissions.CashAccountUpdate));

                options.AddPolicy(Permissions.CashAccountDelete, policy =>
                    policy.RequireClaim(PermissionTypes.CashAccount, Permissions.CashAccountDelete));

                options.AddPolicy(Permissions.CashAccountPage, policy =>
                    policy.RequireClaim(PermissionTypes.CashAccount, Permissions.CashAccountPage));

                // Mobile Account
                options.AddPolicy(Permissions.MobileAccountAdd, policy =>
                    policy.RequireClaim(PermissionTypes.MobileAccount, Permissions.MobileAccountAdd));

                options.AddPolicy(Permissions.MobileAccountUpdate, policy =>
                    policy.RequireClaim(PermissionTypes.MobileAccount, Permissions.MobileAccountUpdate));

                options.AddPolicy(Permissions.MobileAccountDelete, policy =>
                    policy.RequireClaim(PermissionTypes.MobileAccount, Permissions.MobileAccountDelete));

                options.AddPolicy(Permissions.MobileAccountPage, policy =>
                    policy.RequireClaim(PermissionTypes.MobileAccount, Permissions.MobileAccountPage));

                //Category 
                options.AddPolicy(Permissions.CategoryAdd, policy =>
                    policy.RequireClaim(PermissionTypes.Category, Permissions.CategoryAdd));

                options.AddPolicy(Permissions.CategoryUpdate, policy =>
                    policy.RequireClaim(PermissionTypes.Category, Permissions.CategoryUpdate));

                options.AddPolicy(Permissions.CategoryDelete, policy =>
                    policy.RequireClaim(PermissionTypes.Category, Permissions.CategoryDelete));

                options.AddPolicy(Permissions.CategoryPage, policy =>
                    policy.RequireClaim(PermissionTypes.Category, Permissions.CategoryPage));

                //Unit 
                options.AddPolicy(Permissions.UnitAdd, policy =>
                    policy.RequireClaim(PermissionTypes.Unit, Permissions.UnitAdd));

                options.AddPolicy(Permissions.UnitUpdate, policy =>
                    policy.RequireClaim(PermissionTypes.Unit, Permissions.UnitUpdate));

                options.AddPolicy(Permissions.UnitDelete, policy =>
                    policy.RequireClaim(PermissionTypes.Unit, Permissions.UnitDelete));

                options.AddPolicy(Permissions.UnitPage, policy =>
                    policy.RequireClaim(PermissionTypes.Unit, Permissions.UnitPage));

                //User Role 
                options.AddPolicy(Permissions.UserRoleAdd, policy =>
                    policy.RequireClaim(PermissionTypes.UserRole, Permissions.UserRoleAdd));

                options.AddPolicy(Permissions.UserRoleUpdate, policy =>
                    policy.RequireClaim(PermissionTypes.UserRole, Permissions.UserRoleUpdate));

                options.AddPolicy(Permissions.UserRoleDelete, policy =>
                    policy.RequireClaim(PermissionTypes.UserRole, Permissions.UserRoleDelete));

                options.AddPolicy(Permissions.UserRolePage, policy =>
                    policy.RequireClaim(PermissionTypes.UserRole, Permissions.UserRolePage));

                options.AddPolicy(Permissions.UserRoleAssign, policy =>
                    policy.RequireClaim(PermissionTypes.UserRole, Permissions.UserRoleAssign));

                options.AddPolicy(Permissions.UserRoleRemove, policy =>
                    policy.RequireClaim(PermissionTypes.UserRole, Permissions.UserRoleRemove));

                //Department 
                options.AddPolicy(Permissions.DepartmentAdd, policy =>
                    policy.RequireClaim(PermissionTypes.Department, Permissions.DepartmentAdd));

                options.AddPolicy(Permissions.DepartmentUpdate, policy =>
                    policy.RequireClaim(PermissionTypes.Department, Permissions.DepartmentUpdate));

                options.AddPolicy(Permissions.DepartmentDelete, policy =>
                    policy.RequireClaim(PermissionTypes.Department, Permissions.DepartmentDelete));

                options.AddPolicy(Permissions.DepartmentPage, policy =>
                    policy.RequireClaim(PermissionTypes.Department, Permissions.DepartmentPage));

                //Employee 
                options.AddPolicy(Permissions.EmployeeAdd, policy =>
                    policy.RequireClaim(PermissionTypes.Employee, Permissions.EmployeeAdd));

                options.AddPolicy(Permissions.EmployeeUpdate, policy =>
                    policy.RequireClaim(PermissionTypes.Employee, Permissions.EmployeeUpdate));

                options.AddPolicy(Permissions.EmployeeDelete, policy =>
                    policy.RequireClaim(PermissionTypes.Employee, Permissions.EmployeeDelete));

                options.AddPolicy(Permissions.EmployeePage, policy =>
                    policy.RequireClaim(PermissionTypes.Employee, Permissions.EmployeePage));

                //User 
                options.AddPolicy(Permissions.UserAdd, policy =>
                    policy.RequireClaim(PermissionTypes.User, Permissions.UserAdd));

                options.AddPolicy(Permissions.UserUpdate, policy =>
                    policy.RequireClaim(PermissionTypes.User, Permissions.UserUpdate));

                options.AddPolicy(Permissions.UserDelete, policy =>
                    policy.RequireClaim(PermissionTypes.User, Permissions.UserDelete));

                options.AddPolicy(Permissions.UserPage, policy =>
                    policy.RequireClaim(PermissionTypes.User, Permissions.UserPage));

                // Access
                options.AddPolicy(Permissions.AccessSetup, policy =>
                    policy.RequireClaim(PermissionTypes.AccessSetup, Permissions.AccessSetup));

                // Settings
                options.AddPolicy(Permissions.SettingsPage, policy =>
                    policy.RequireClaim(PermissionTypes.Settings, Permissions.SettingsPage));
            });
        }

        public static void AddCookieAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication()
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = new PathString("/Account/Login");
                    options.AccessDeniedPath = new PathString("/Account/Login");
                    options.LogoutPath = new PathString("/Account/Logout");
                    options.Cookie.Name = "DevSkill.Identity";
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                });
        }
    }
}
