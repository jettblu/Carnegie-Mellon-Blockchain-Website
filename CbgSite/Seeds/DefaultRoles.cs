using CbgSite.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CbgSite.Seeds
{
    public class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<CbgUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Globals.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Globals.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Globals.Roles.Basic.ToString()));
        }
    }
}
