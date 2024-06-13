using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POC3.Application;
using POC3.services;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(Options=>Options.UseSqlServer("Server=G40XMS3\\SQLSERVER2022 ; Database= Demo1DB ; Trusted_Connection=True; TrustServerCertificate=True;"));

builder.Services.AddScoped<IUserService, UserService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

// app.UseAuthentication();

app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();




// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.OpenApi.Models;
// using POC.Application;
// using Swashbuckle.AspNetCore; 

// var builder = WebApplication.CreateBuilder(args);

// // Configuration
// IConfiguration configuration = builder.Configuration;

// // Add services to the container.
// builder.Services.AddControllersWithViews();

// // Add DbContext with connection string from configuration
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

// // Register Swagger services
// builder.Services.AddSwaggerGen(c =>
// {
//     c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
// });

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (!app.Environment.IsDevelopment())
// {
//     app.UseExceptionHandler("/Home/Error");
//     app.UseHsts();
// }

// app.UseHttpsRedirection();
// app.UseStaticFiles();

// app.UseRouting();

// app.UseAuthorization();

// // Enable middleware to serve generated Swagger as OpenAPI document
// app.UseSwagger();

// // Enable middleware to serve Swagger UI at the root path /swagger
// app.UseSwaggerUI(c =>
// {
//     c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
// });

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}");

// app.Run();
