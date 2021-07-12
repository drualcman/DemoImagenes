using DemoImagenes.Client.IndexedDb;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DemoImagenes.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            CultureInfo.CurrentCulture = new CultureInfo("es-ES");
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("es-ES");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("es-ES");

            builder.Services.AddSingleton<DBContext>();

            await builder.Build().RunAsync();
        }
    }
}
