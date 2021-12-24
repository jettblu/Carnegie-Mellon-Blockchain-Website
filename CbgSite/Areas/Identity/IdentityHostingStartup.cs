using System;
using CbgSite.Areas.Identity.Data;
using CbgSite.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(CbgSite.Areas.Identity.IdentityHostingStartup))]
namespace CbgSite.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<CbgSiteContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("CbgSiteContextConnection")));

                services.AddDefaultIdentity<CbgUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<CbgSiteContext>();
            });
        }
    }
}