using Microsoft.EntityFrameworkCore;
using Vouchers.Persistence;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddDbContext<VouchersDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("VouchersDbContextConnection")));

var app = builder.Build();

app.Run();