using System.Globalization;
using Application.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Vouchers.Application.Events.IdentityEvents;
using Vouchers.MinimalAPI.Endpoints;
using Vouchers.Application.Infrastructure;
using Vouchers.EntityFramework;
using Vouchers.Infrastructure;
using Vouchers.MinimalAPI.Binding;
using Vouchers.MinimalAPI.EventRouters;
using Vouchers.MinimalAPI.Services;
using Vouchers.MinimalAPI.Validation;

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

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IDispatcher, Dispatcher>();
builder.Services.AddScoped<ILoginNameProvider, JWTLoginNameProvider>();
builder.Services.AddScoped<IImageService, ImageSharpService>();
builder.Services.AddScoped<ICultureInfoProvider, CultureInfoProvider>();

builder.Services.AddEventRouter<GenericEventRouter<IdentityUpdatedEvent>>(nameof(IdentityUpdatedEvent));
builder.Services.AddScoped<IMessageDataSerializer, MessageDataSerializer>();
builder.Services.AddHostedService<OutboxMessagesProcessingService>();


builder.Services.AddRepositories();
builder.Services.AddCommandHandlers();
builder.Services.AddQueryHandlers();

builder.Services.AddFormValidators();
builder.Services.AddFormParameterProviders();

//builder.Services.AddControllers(); //use MinimalAPI

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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