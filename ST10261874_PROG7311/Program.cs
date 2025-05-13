using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ST10261874_PROG7311.Data;
using ST10261874_PROG7311.Models;

var builder = WebApplication.CreateBuilder(args);

//add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//make login session expire when app is stopped (no persistent cookies)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20); // session timeout
    options.SlidingExpiration = true;
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.Name = ".ST10261874_PROG7311.Auth";
    options.LoginPath = "/Home/Login";
    options.LogoutPath = "/Home/Logout";
    options.AccessDeniedPath = "/Home/AccessDenied";
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

//seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


//goes to respective controller and action
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "loginRoute",
    pattern: "Home/{action=Login}/{id?}",
    defaults: new { controller = "Home", action = "Login" });

app.MapControllerRoute(
    name: "addFarmerRoute",
    pattern: "Employee/{action=AddFarmer}/{id?}",
    defaults: new { controller = "Employee", action = "AddFarmer" });

app.MapControllerRoute(
    name: "viewFarmersRoute",
    pattern: "Employee/{action=ViewFarmers}/{id?}",
    defaults: new { controller = "Employee", action = "ViewFarmers" });

app.MapControllerRoute(
    name: "farmerIndexRoute",
    pattern: "Farmer/{action=Index}/{id?}",
    defaults: new { controller = "Farmer", action = "Index" });

app.MapRazorPages();

app.Run();
