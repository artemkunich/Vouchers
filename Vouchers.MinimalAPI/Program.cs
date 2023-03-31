using System.Globalization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Vouchers.Domains.Application.Infrastructure;
using Vouchers.Common.Application.Abstractions;
using Vouchers.MinimalAPI.Endpoints;
using Vouchers.Common.Application.Infrastructure;
using Vouchers.Core.Domain;
using Vouchers.Domains.Domain;
using Vouchers.Files.Domain;
using Vouchers.Persistence;
using Vouchers.Infrastructure;
using Vouchers.MinimalAPI.Binding;
using Vouchers.MinimalAPI.Services;
using Vouchers.MinimalAPI.Validation;
using Vouchers.Persistence.InterCommunication;
using Vouchers.Resources;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<VouchersDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("VouchersDbContextConnection")));

builder.Services.AddLocalization();
builder.Services.Configure<RequestLocalizationOptions>(
    options => {

        options.DefaultRequestCulture = new RequestCulture(CultureInfo.InvariantCulture);

        options.SupportedCultures = new[] { CultureInfo.InvariantCulture };
        options.SupportedUICultures =  new[] { CultureInfo.InvariantCulture, new CultureInfo("en-US"), new CultureInfo("cs-CZ") };

        options.RequestCultureProviders = new[] { new AcceptLanguageHeaderRequestCultureProvider() };
    }
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

builder.Services.AddCors(options =>
{
    // this defines a CORS policy called "default"
    options.AddPolicy("default", policy =>
    {
        policy.WithOrigins("http://localhost:8080")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EmailId", policy => policy.RequireClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"));
    options.AddPolicy("RoleAdmin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RoleUser", policy => policy.RequireRole("User"));
    options.AddPolicy("RoleManager", policy => policy.RequireRole("Manager"));
    
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "vouapi");
        //policy.RequireScope("vouapi");
    });
});

builder.Services
    .AddHttpContextAccessor()
    .AddScoped<ILoginNameProvider, JWTLoginNameProvider>()
    .AddScoped<IImageService, ImageSharpService>()
    .AddScoped<ICultureInfoProvider, CultureInfoProvider>()
    
    .AddHostedService<OutboxMessagesProcessingService>()
    
    .AddRepositoryForEntities(typeof(VouchersDbContext).Assembly) //Persistance
    .AddRepositoryForEntities(typeof(Account).Assembly) //Core
    .AddRepositoryForEntities(typeof(Domain).Assembly) //Domains
    .AddRepositoryForEntities(typeof(CroppedImage).Assembly) //Files
    .AddRepositoryForEntities(typeof(Identity).Assembly) //Identities
    .AddRepositoryForEntities(typeof(VoucherValue).Assembly) //Values
    .AddInfrastructureServices()
    .AddApplicationServices()
    .AddRequestHandlers(typeof(IRequestHandler<,>).Assembly)
    .AddRequestHandlers(typeof(VouchersDbContext).Assembly)
    .AddDomainEventHandlers(typeof(IDomainEventHandler<>).Assembly)
    .AddIntegrationEventHandlers(typeof(IDomainEventHandler<>).Assembly)
    .AddRequestPipelineBehaviors(typeof(IRequestHandler<,>).Assembly)
    .AddEventPipelineBehaviors(typeof(IIntegrationEventPipeline<>).Assembly)
    .AddGenericPipeline()
    
    .AddFormValidators()
    .AddFormParameterProviders()

//builder.Services.AddControllers(); //use MinimalAPI

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
    {
        var vouchersContext = serviceScope.ServiceProvider.GetRequiredService<VouchersDbContext>();
        vouchersContext.Database.Migrate();
    }
}

app.UseCors("default");

var localizeOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
if(localizeOptions is not null)
    app.UseRequestLocalization(localizeOptions.Value);

app.UseAuthentication();
app.UseAuthorization();

app.MapDomainAccountEndpoints();
app.MapDomainDetailEndpoints();
app.MapIdentityDetailEndpoints();
app.MapIdentityDomainAccountEndpoints();


app.Run();