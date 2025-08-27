using HPCTechSummer2025MovieApp.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDAwODEyMkAzMzMwMmUzMDJlMzAzYjMzMzAzYmFYaUMyMHkweFN3MzNGTHJuMWVlMmJDUGhUaG02M1Bkbk13MjBUa0hib3M9");


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddHttpClient();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
builder.Services.AddSyncfusionBlazor();
await builder.Build().RunAsync();
