using BrowserLlamaCpp.Client;
using BrowserLlamaCpp.Client.JsInteropServices;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var services = builder.Services;
services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["API_Prefix"] ?? builder.HostEnvironment.BaseAddress) });
services.AddHttpClient<FunctionsClient>(sp =>
{
	sp.BaseAddress = new Uri(builder.Configuration["API_Prefix"] ?? builder.HostEnvironment.BaseAddress);	
});
services.AddJsInteropServices();
services.AddRadzenComponents();
await builder.Build().RunAsync();

namespace BrowserLlamaCpp.Client
{
}