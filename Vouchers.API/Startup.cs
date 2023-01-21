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
using Vouchers.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Vouchers.Application.Infrastructure;
using Vouchers.API.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;
using Vouchers.Infrastructure;

namespace Vouchers.API;

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

        services.AddLocalization();
        services.Configure<RequestLocalizationOptions>(
            options => {

                options.DefaultRequestCulture = new RequestCulture(CultureInfo.InvariantCulture);

                options.SupportedCultures = new[] { CultureInfo.InvariantCulture };
                options.SupportedUICultures =  new[] { CultureInfo.InvariantCulture, new CultureInfo("en-US"), new CultureInfo("cs-CZ") };

                options.RequestCultureProviders = new[] { new AcceptLanguageHeaderRequestCultureProvider() };
            }
        );

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("Bearer", options =>
            {
                options.RequireHttpsMetadata = false;
                options.Authority = "http://vouchers.identity-server";
                //options.Authority = "http://localhost:5000";                    

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };
            });
        //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => Configuration.Bind("CookieSettings", options));

        services.AddCors(options =>
        {
            // this defines a CORS policy called "default"
            options.AddPolicy("default", policy =>
            {
                policy.WithOrigins("http://localhost:8080")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vouchers.Web", Version = "v1" });
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("EmailId", policy => policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"));
            options.AddPolicy("RoleAdmin", policy => policy.RequireRole("Admin"));
            options.AddPolicy("ApiScope", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "vouapi");
            });
        });

        services
            .AddHttpContextAccessor()

            .AddScoped<IDispatcher, Dispatcher>()
            .AddScoped<ILoginNameProvider, JWTLoginNameProvider>()
            .AddScoped<IImageService, ImageSharpService>()
            .AddScoped<ICultureInfoProvider, CultureInfoProvider>()
            
            .AddHostedService<OutboxMessagesProcessingService>()
                
            .AddRepositories(Configuration)
            .AddAppServices(Configuration)
            .AddHandlers(Configuration);

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
                var vouchersContext = serviceScope.ServiceProvider.GetRequiredService<VouchersDbContext>();
                vouchersContext.Database.Migrate();
            }
        }

        app.UseCors("default");

        var localizeOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
        if(localizeOptions is not null)
            app.UseRequestLocalization(localizeOptions.Value);

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers().RequireAuthorization("ApiScope");
        });
    }
}