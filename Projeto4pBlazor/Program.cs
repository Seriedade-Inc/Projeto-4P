using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Projeto4pBlazor;
using TG.Blazor.IndexedDB;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

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

await builder.Build().RunAsync();