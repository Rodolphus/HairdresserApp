using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HairdresserApp.Data;
using HairdresserApp.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("HairdresserAppContextConnection") ?? throw new InvalidOperationException("Connection string 'HairdresserAppContextConnection' not found.");;

builder.Services.AddDbContext<HairdresserAppContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<HairdresserAppUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<HairdresserAppContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireDigit = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "User" };

    foreach (var role in roles)
    {
        if(!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<HairdresserAppUser>>();
    string firtName = "Mehmet Akif";
    string lastName = "Turhan";
    string email = "b181210090@sakarya.edu.tr";
    string password = "sau";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new HairdresserAppUser();
        user.FirstName = firtName;
        user.LastName = lastName;
        user.UserName = email;
        user.Email = email;
        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Admin");
    }
}

app.Run();