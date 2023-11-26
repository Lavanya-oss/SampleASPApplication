using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentsProfileApplication.Data;
using StudentsProfileApplication.Interface;
using StudentsProfileApplication.Models;
using StudentsProfileApplication.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<StudentsInfoDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("StduentDBString")));
builder.Services.AddDbContext<ProfileAuditDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("constring")));


builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ProfileAuditDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
