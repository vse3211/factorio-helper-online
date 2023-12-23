using BlazorBootstrap;
using FHW.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddHubOptions(options => {
    options.MaximumReceiveMessageSize = null;
});
builder.Services.AddBlazorBootstrap();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
Temp.LoadModsList();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseMvcWithDefaultRoute();

app.UseStaticFiles();
if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Files")))
    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Files"));
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Files")),
    RequestPath = "/Files"
});

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
