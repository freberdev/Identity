using IdentityDemo.Models;
using IdentityDemo.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<AccountService>();

var connString = builder.Configuration
    .GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MyContext>(o => o.UseSqlServer(connString));
builder.Services.AddDbContext<IdentityDbContext>(o => o.UseSqlServer(connString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(
    o => o.LoginPath = "/login");

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); // Identifiering
app.UseAuthorization(); // Behörighet

app.UseEndpoints(o => o.MapControllers());

app.Run();