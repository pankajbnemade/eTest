using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(ERP.UI.Areas.Identity.IdentityHostingStartup))]
namespace ERP.UI.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}