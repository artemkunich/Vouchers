using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Vouchers.Auth;
using Vouchers.IdentityServer;
using Vouchers.IdentityServer.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbContextConnection")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 9;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 20;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;

}).AddEntityFrameworkStores<AuthDbContext>().AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();

var idBuilder = builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;

    options.IssuerUri = "http://vouchers.identityserver";

    // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
    options.EmitStaticAudienceClaim = true;
})
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryClients(Config.Clients)
    .AddAspNetIdentity<ApplicationUser>()
    .AddProfileService<ProfileService>();

idBuilder.AddDeveloperSigningCredential();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
    options.OnAppendCookie = cookieContext =>
        CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
    options.OnDeleteCookie = cookieContext =>
        CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
});

builder.Services.AddCors(options =>
{
    // this defines a CORS policy called "default"
    options.AddPolicy("default", policy =>
    {
        policy.WithOrigins("http://localhost:6000")
           .AllowAnyHeader()
           .AllowAnyMethod();
        policy.WithOrigins("https://localhost:6001")
           .AllowAnyHeader()
           .AllowAnyMethod();
        policy.WithOrigins("http://localhost:8080")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});



//builder.Services.AddAuthentication();
//.AddGoogle("Google", options =>
//{
//    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

//    options.ClientId = "<insert here>";
//    options.ClientSecret = "<insert here>";
//})
//.AddOpenIdConnect("oidc", "Demo IdentityServer", options =>
//{
//    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
//    options.SignOutScheme = IdentityServerConstants.SignoutScheme;
//    options.SaveTokens = true;

//    options.Authority = "https://demo.identityserver.io/";
//    options.ClientId = "interactive.confidential";
//    options.ClientSecret = "secret";
//    options.ResponseType = "code";

//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        NameClaimType = "name",
//        RoleClaimType = "role"
//    };
//});




var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
    {
        var authContext = serviceScope.ServiceProvider.GetRequiredService<AuthDbContext>();
        authContext.Database.Migrate();

        var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var task = InsureCreateRoles(configuration, userManager, roleManager);
        task.Wait();
    }
}

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.UseRouting();

app.UseCors("default");

app.UseIdentityServer();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

app.Run();



void CheckSameSite(HttpContext httpContext, CookieOptions options)
{
    if (options.SameSite == SameSiteMode.None)
    {
        var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
        options.SameSite = SameSiteMode.Unspecified;
    }
}

async Task InsureCreateRoles(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
{
    string[] roleNames = { "Admin", "Manager", "User" };
    IdentityResult roleResult;

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
            //create the roles and seed them to the database: Question 1
            roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    var adminEmail = configuration["AppSettings:AdminEmail"];
    var adminName = adminEmail;
    var adminPassword = configuration["AppSettings:AdminPassword"];

    if (adminEmail == null || adminPassword == null)
        return;

    var admin = new ApplicationUser
    {
        UserName = adminName,
        Email = adminEmail,
    };

    var _admin = await userManager.FindByEmailAsync(adminEmail);

    if (_admin == null)
    {
        var createAdminResult = await userManager.CreateAsync(admin, adminPassword);
        if (createAdminResult.Succeeded)
        {
            //here we tie the new user to the role
            await userManager.AddToRoleAsync(admin, "Admin");

        }
    }
}
