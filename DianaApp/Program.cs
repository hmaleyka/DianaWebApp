using DianaApp.DAL;
using DianaApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.Password.RequireNonAlphanumeric = true;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
    opt.Lockout.MaxFailedAccessAttempts = 3;
    opt.User.RequireUniqueEmail = true;
    opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._+";
}).AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();



var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
     name: "Manage",
     pattern: "{area:exists}/{controller=DashBoard}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}"

    );

app.UseStaticFiles();
app.Run();
