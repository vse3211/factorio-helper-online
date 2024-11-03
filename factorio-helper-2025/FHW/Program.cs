using FHW.Components;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

//FHW.Data.Temp.LoadModsList();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddFluentUIComponents(options =>
{
    options.ValidateClassNames = false;
});

builder.Services.AddHttpClient<FHW.Services.FactorioService>((serviceProvider, httpClient) => {
    httpClient.BaseAddress = new Uri("https://factorio.com");
});
builder.Services.AddHttpClient<FHW.Services.FactorioUpdateService>((serviceProvider, httpClient) => {
    httpClient.BaseAddress = new Uri("https://updater.factorio.com");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();