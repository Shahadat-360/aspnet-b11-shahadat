using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevSkill.Inventory.Infrastructure.Seeds
{
    public static class ClaimSeed
    {
        public static ApplicationRoleClaim[] GetClaims()
        {
            var claimId = 1;
            var adminRoleId = new Guid("79149158-E28B-4EC3-B110-98CD62CB58BF");

            Dictionary<string, string> permissions = new()
            {
                { Permissions.CustomerAdd, PermissionTypes.Customer },
                { Permissions.CustomerUpdate, PermissionTypes.Customer },
                { Permissions.CustomerDelete, PermissionTypes.Customer },
                { Permissions.CustomerPage, PermissionTypes.Customer },

                { Permissions.ProductAdd, PermissionTypes.Product },
                { Permissions.ProductUpdate, PermissionTypes.Product },
                { Permissions.ProductDelete, PermissionTypes.Product },
                { Permissions.ProductPage, PermissionTypes.Product },

                { Permissions.SaleAdd, PermissionTypes.Sale },
                { Permissions.SaleUpdate, PermissionTypes.Sale },
                { Permissions.SaleDelete, PermissionTypes.Sale },
                { Permissions.SalePage, PermissionTypes.Sale },

                { Permissions.BalanceTransferAdd, PermissionTypes.BalanceTransfer },
                { Permissions.BalanceTransferDelete, PermissionTypes.BalanceTransfer },
                { Permissions.BalanceTransferPage, PermissionTypes.BalanceTransfer },

                { Permissions.EmployeeAdd, PermissionTypes.Employee },
                { Permissions.EmployeeUpdate, PermissionTypes.Employee },
                { Permissions.EmployeeDelete, PermissionTypes.Employee },
                { Permissions.EmployeePage, PermissionTypes.Employee },

                { Permissions.UserAdd, PermissionTypes.User },
                { Permissions.UserUpdate, PermissionTypes.User },
                { Permissions.UserDelete, PermissionTypes.User },
                { Permissions.UserPage, PermissionTypes.User },

                { Permissions.CategoryAdd, PermissionTypes.Category },
                { Permissions.CategoryUpdate, PermissionTypes.Category },
                { Permissions.CategoryDelete, PermissionTypes.Category },
                { Permissions.CategoryPage, PermissionTypes.Category },

                { Permissions.UnitAdd, PermissionTypes.Unit },
                { Permissions.UnitUpdate, PermissionTypes.Unit },
                { Permissions.UnitDelete, PermissionTypes.Unit },
                { Permissions.UnitPage, PermissionTypes.Unit },

                { Permissions.DepartmentAdd, PermissionTypes.Department },
                { Permissions.DepartmentUpdate, PermissionTypes.Department },
                { Permissions.DepartmentDelete, PermissionTypes.Department },
                { Permissions.DepartmentPage, PermissionTypes.Department },

                { Permissions.CashAccountAdd, PermissionTypes.CashAccount },
                { Permissions.CashAccountUpdate, PermissionTypes.CashAccount },
                { Permissions.CashAccountDelete, PermissionTypes.CashAccount },
                { Permissions.CashAccountPage, PermissionTypes.CashAccount },

                { Permissions.BankAccountAdd, PermissionTypes.BankAccount },
                { Permissions.BankAccountUpdate, PermissionTypes.BankAccount },
                { Permissions.BankAccountDelete, PermissionTypes.BankAccount },
                { Permissions.BankAccountPage, PermissionTypes.BankAccount },

                { Permissions.MobileAccountAdd, PermissionTypes.MobileAccount },
                { Permissions.MobileAccountUpdate, PermissionTypes.MobileAccount },
                { Permissions.MobileAccountDelete, PermissionTypes.MobileAccount },
                { Permissions.MobileAccountPage, PermissionTypes.MobileAccount },

                { Permissions.UserRoleAdd, PermissionTypes.UserRole },
                { Permissions.UserRoleUpdate, PermissionTypes.UserRole },
                { Permissions.UserRoleDelete, PermissionTypes.UserRole },
                { Permissions.UserRolePage, PermissionTypes.UserRole },
                { Permissions.UserRoleAssign, PermissionTypes.UserRole },
                { Permissions.UserRoleRemove, PermissionTypes.UserRole },

                {Permissions.SettingsPage, PermissionTypes.Settings },
                { Permissions.AccessSetup, PermissionTypes.AccessSetup }
            };

            var roleClaims = permissions.Select(kvp => new ApplicationRoleClaim
            {
                Id = claimId++,
                RoleId = adminRoleId,
                ClaimType = kvp.Value,
                ClaimValue = kvp.Key  
            });

            return [.. roleClaims];
        }
    }
}
