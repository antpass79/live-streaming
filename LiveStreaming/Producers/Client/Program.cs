using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using BlazorProducer.Shared.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorProducer.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<IStreamingHubProxy<string>, StreamingHubProxy<string>>();

            await builder.Build().RunAsync();
        }
    }
}
