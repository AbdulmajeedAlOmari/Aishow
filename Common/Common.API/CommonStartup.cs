using Common.Clients.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Common.CommonStartup))]
namespace Common
{
    public class CommonStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                Console.WriteLine("Common Startup Successful.");

                // Inject System.Net.Http.HttpClient instance to the DIC
                services.AddHttpClient<System.Net.Http.HttpClient>();
            });
        }
    }
}
