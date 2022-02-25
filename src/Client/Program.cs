using AuctionMarket.Client;
using AuctionMarket.Client.Application.Abstractions;
using AuctionMarket.Client.Application.Commands;
using AuctionMarket.Client.Application.Services;
using AuctionMarket.Client.Infrastructure.Services;
using Blazored.LocalStorage;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<AppComponent>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddSingleton<IHubConnectionService, HubConnectionService>();
builder.Services.AddSingleton<IStateContainerService, StateContainerService>();
builder.Services.AddSingleton<AuthenticationStateProvider, HostAuthenticationStateProvider>();

builder.Services.AddSingleton<IHostAuthenticationStateProvider>(serviceProvider
    => (HostAuthenticationStateProvider)serviceProvider.GetRequiredService<AuthenticationStateProvider>());

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();
builder.Services.AddMediatR(typeof(LoginAccountCommandHandler));

await builder.Build().RunAsync();