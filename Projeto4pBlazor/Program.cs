using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Projeto4pBlazor;
using TG.Blazor.IndexedDB;
using Projeto4pSharedLibrary.Services;
using Microsoft.JSInterop;
using System.Reflection.Metadata.Ecma335;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
BuilderService buildingService = new();
//if (buildingService.CheckBlazorWebAuth()==true){return;}else{}
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddSingleton<UserSession>();

// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddIndexedDB(dbStore =>
{
    dbStore.DbName = "CarinomiconLocalDb";
    dbStore.Version = 1;
    dbStore.Stores.Add(new StoreSchema
    {
        Name = "CustomItems",
        PrimaryKey = new IndexSpec { Name = "id", KeyPath = "id", Auto = true }
    });
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5149/") });

await builder.Build().RunAsync();