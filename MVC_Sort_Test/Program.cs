using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MVC_Sort_Test.Data;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MVC_Sort_TestContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MVC_Sort_TestContext") ?? throw new InvalidOperationException("Connection string 'MVC_Sort_TestContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.KnownProxies.Add(IPAddress.Parse("10.0.0.10"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{   
    //IF NOT DEVELOPMENT
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });
}
else
{
    //IF IN DEVELOPMENT
    app.UseHttpsRedirection();

}


app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
