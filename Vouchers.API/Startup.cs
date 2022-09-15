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
using Vouchers.Application.Commands;
using Vouchers.Application.UseCases;
using Vouchers.Application.Queries;
using Vouchers.Application.Dtos;
using Vouchers.EntityFramework.QueryHandlers;
using Vouchers.Application.Infrastructure;
using Vouchers.EntityFramework.Repositories;
using Vouchers.Identities;
using Vouchers.API.Services;
using Vouchers.Auth;
using Microsoft.IdentityModel.Tokens;


namespace Vouchers.API
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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("Bearer", options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.Authority = "http://vouchers.identityserver";
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

            services.AddHttpContextAccessor();

            services.AddScoped<IDispatcher, Dispatcher>();

            services.AddScoped<ILoginService, JWTLoginService>();

            services.AddScoped<IImageService, ImageSharpService>();

            //Repositories
            services.AddRepositories();

            //Identity
            services.AddHandler<string, Guid?, IdentityQueryHandler>();
            services.AddAuthIdentityHandler<Guid?, IdentityDetailDto, IdentityDetailQueryHandler>();

            services.AddHandler<CreateIdentityCommand, CreateIdentityCommandHandler>();           
            services.AddAuthIdentityHandler<UpdateIdentityCommand, UpdateIdentityCommandHandler>();


            //Domain offers
            services.AddHandler<DomainOffersQuery, IEnumerable<DomainOfferDto>, DomainOffersQueryHandler>();
            services.AddAuthIdentityHandler<IdentityDomainOffersQuery, IEnumerable<DomainOfferDto>, IdentityDomainOffersQueryHandler>();

            services.AddAuthIdentityHandler<CreateDomainOfferCommand, Guid, CreateDomainOfferCommandHandler>();
            services.AddAuthIdentityHandler<UpdateDomainOfferCommand, UpdateDomainOfferCommandHandler>();
            

            services.AddAuthIdentityHandler<CreateDomainCommand, Guid?, CreateDomainCommandHandler>();

            //Domains
            services.AddAuthIdentityHandler<DomainsQuery, IEnumerable<DomainDto>, DomainsQueryHandler>();

            //Domain accounts
            services.AddAuthIdentityHandler<DomainAccountsQuery, IEnumerable<DomainAccountDto>, DomainAccountsQueryHandler>();
            services.AddAuthIdentityHandler<IdentityDomainAccountsQuery, IEnumerable<DomainAccountDto>, IdentityDomainAccountsQueryHandler>();
            services.AddAuthIdentityHandler<CreateDomainAccountCommand, Guid, CreateDomainAccountCommandHandler>();

            services.AddAuthIdentityHandler<UpdateDomainAccountCommand, UpdateDomainAccountCommandHandler>();            

            //Issuer values
            services.AddAuthIdentityHandler<IssuerValuesQuery, IEnumerable<VoucherValueDto>, IssuerValuesQueryHandler>();

            services.AddAuthIdentityHandler<CreateVoucherValueCommand, Guid, CreateVoucherValueCommandHandler>();
            services.AddAuthIdentityHandler<UpdateVoucherValueCommand, UpdateVoucherValueCommandHandler>();
            services.AddAuthIdentityHandler<Guid, VoucherValueDetailDto, VoucherValueDetailQueryHandler>();

            //Issuer vouchers
            services.AddAuthIdentityHandler<IssuerVouchersQuery, IEnumerable<VoucherDto>, IssuerVouchersQueryHandler>();
            services.AddAuthIdentityHandler<CreateVoucherCommand, Guid, CreateVoucherCommandHandler>();
            services.AddAuthIdentityHandler<UpdateVoucherCommand, UpdateVoucherCommandHandler>();

            //Issuer transactions
            services.AddAuthIdentityHandler<IssuerTransactionsQuery, IEnumerable<IssuerTransactionDto>, IssuerTransactionsQueryHandler>();

            services.AddAuthIdentityHandler<CreateIssuerTransactionCommand, Guid, CreateIssuerTransactionCommandHandler>();

            //Holder values
            services.AddAuthIdentityHandler<HolderValuesQuery, IEnumerable<VoucherValueDto>, HolderValuesQueryHandler>();
            

            //Holder vouchers
            services.AddAuthIdentityHandler<HolderVouchersQuery, IEnumerable<VoucherDto>, HolderVouchersQueryHandler>();

            //Holder transactions
            services.AddAuthIdentityHandler<CreateHolderTransactionCommand, Guid, CreateHolderTransactionCommandHandler>();
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
        
            app.UseAuthentication();
            app.UseAuthorization();          

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization("ApiScope");
            });
        }
    }
}
