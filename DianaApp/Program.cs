using DianaApp.DAL;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllersWithViews();




builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});



var app = builder.Build();
app.MapControllerRoute(
     name: "Manage",
     pattern: "{area:exists}/{controller=DashBoard}/{action=Index}/{id?}");




app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}"

    );

app.UseStaticFiles();
app.Run();
