using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vouchers.EntityFramework;
using Vouchers.Auth;
using Microsoft.AspNetCore.Identity;
using Vouchers.MVC.Extensions;
using Vouchers.Application.Infrastructure;
using Vouchers.EntityFramework.Repositories;
using Vouchers.EntityFramework.QueryHandlers;
using Vouchers.Identities;
using Vouchers.Core;
using Vouchers.Application.UseCases;
using Vouchers.Application.Dtos;
using Vouchers.Application.Queries;
using Vouchers.MVC.Services;
using AutoMapper;

namespace Vouchers.MVC
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
                    .AddDefaultUI()
                    .AddEntityFrameworkStores<AuthDbContext>()
                    .AddDefaultTokenProviders();

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


            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddAuthorization(options =>
                {
                    options.AddPolicy("EmailId", policy => policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"));
                    options.AddPolicy("RoleAdmin", policy => policy.RequireRole("Admin"));
                }
            );

            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton<IMapper<IdentityDetail, IdentityDetailDto>, Mapper<IdentityDetail, IdentityDetailDto>>();

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
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var authContext = serviceScope.ServiceProvider.GetRequiredService<AuthDbContext>();
                authContext.Database.Migrate();

                var vouchersContext = serviceScope.ServiceProvider.GetRequiredService<VouchersDbContext>();
                vouchersContext.Database.Migrate();                
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.UseAuthentication();
            app.UseAuthorization();

            var task = CreateRoles(userManager, roleManager);
            task.Wait();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapRazorPages();
            });
        }


        private async Task CreateRoles(
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
