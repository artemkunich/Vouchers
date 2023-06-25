using Akunich.Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Vouchers.Files.Application.Infrastructure;
using Vouchers.Files.Application.ServiceProviders;
using Vouchers.Files.Application.Services;

namespace Vouchers.Files.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFilesModule(this IServiceCollection serviceCollection) => serviceCollection
        .AddScoped<IImageService, ImageSharpService>()
        .AddScoped<IAppImageService, AppImageService>()
        .AddApplication(typeof(Application.UseCases.ImageCases.CreateImageCommand).Assembly);
}