using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Vouchers.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Vouchers.Application.UseCases;
using Vouchers.Application.Queries;
using Vouchers.Application.Dtos;
using Vouchers.EntityFramework.QueryHandlers;
using Vouchers.Application.Infrastructure;
using Vouchers.EntityFramework.Repositories;
using Vouchers.Identities;
using Vouchers.MVC.Services;
using Vouchers.Auth;

namespace Vouchers.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<VouchersDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("VouchersDbContextConnection")));

            services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AuthDbContextConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<AuthDbContext>()
                    .AddDefaultTokenProviders();
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => Configuration.Bind("JwtSettings", options))
            //    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => Configuration.Bind("CookieSettings", options));
            //services.AddIdentity<User, Role>(options => 
            //{
            //    options.Password.RequireDigit = true;
            //    options.Password.RequireLowercase = true;
            //    options.Password.RequireNonAlphanumeric = true;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequiredLength = 9;
            //    options.Password.RequiredUniqueChars = 1;

            //    // Lockout settings.
            //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //    options.Lockout.MaxFailedAccessAttempts = 5;
            //    options.Lockout.AllowedForNewUsers = true;

            //    // User settings.
            //    options.User.AllowedUserNameCharacters =
            //    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            //    options.User.RequireUniqueEmail = true;

            //}).AddEntityFrameworkStores<IdentityDbContext>().AddDefaultTokenProviders();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vouchers.Web", Version = "v1" });
            });

            services.AddAuthorization(options =>
                {
                    options.AddPolicy("EmailId", policy => policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"));
                    options.AddPolicy("RoleAdmin", policy => policy.RequireRole("Admin"));
                }
            );

            services.AddHttpContextAccessor();




            services.AddScoped<IRequestDispatcher, RequestDispatcher>();

            services.AddScoped<ILoginService, LoginService>();

            services.AddScoped<ILoginRepository, LoginRepository>();

            services.AddScoped<IIdentityDetailRepository, IdentityDetailRepository>();

            //Create identity
            services.AddScoped<IHandler<CreateIdentityCommand>, CreateIdentityCommandHandler>();

            //Get identity details
            services.AddScoped<IAuthIdentityHandler<Guid?, IdentityDetailDto>, IdentityDetailQueryHandler>();
            services.AddScoped<IHandler<Guid?, IdentityDetailDto>, AuthIdentityHandler<Guid?, IdentityDetailDto>>();

            //Update identity details
            services.AddScoped<IAuthIdentityHandler<UpdateIdentityDetailCommand>, UpdateIdentityDetailCommandHandler>();
            services.AddScoped<IHandler<UpdateIdentityDetailCommand>, AuthIdentityHandler<UpdateIdentityDetailCommand>>();

            services.AddScoped<IAuthIdentityHandler<SubscriptionsQuery, IEnumerable<SubscriptionDto>>, SubscriptionsQueryHandler>();
            services.AddScoped<AuthIdentityHandler<SubscriptionsQuery, IEnumerable<SubscriptionDto>>>();

            services.AddScoped<IHandler<DomainOffersQuery, IPaginatedEnumerable<DomainOfferDto>>, DomainOffersQueryHandler>();
            services.AddScoped<IHandler<LoginsQuery, IPaginatedEnumerable<LoginDto>>, LoginsQueryHandler>();

            services.AddScoped<IHandler<string, Guid?>, IdentityQueryHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vouchers.Web v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            using (var scope = app.ApplicationServices.CreateScope())
            {
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var authContext = serviceScope.ServiceProvider.GetRequiredService<AuthDbContext>();
                    authContext.Database.Migrate();

                    var vouchersContext = serviceScope.ServiceProvider.GetRequiredService<VouchersDbContext>();
                    vouchersContext.Database.Migrate();

                }
            }

            app.UseAuthentication();
            app.UseAuthorization();

            var task = InsureCreateRoles(userManager, roleManager);
            task.Wait();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private async Task InsureCreateRoles(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
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

            var adminEmail = Configuration["AppSettings:AdminEmail"];
            var adminName = adminEmail;
            var adminPassword = Configuration["AppSettings:AdminPassword"];

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
    }
}
