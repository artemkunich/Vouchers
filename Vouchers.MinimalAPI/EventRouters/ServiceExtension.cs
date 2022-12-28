using Vouchers.Application.UseCases;

namespace Vouchers.MinimalAPI.EventRouters;

public static class ServiceExtension
{
    private static readonly Dictionary<string, Type> Routs = new();
    
    public static void AddEventRouter<TRouter>(this IServiceCollection serviceCollection, string routeName) where TRouter: IEventRouter
    {
        serviceCollection.AddScoped(typeof(TRouter));
        Routs[routeName] = typeof(TRouter);
    }
    
    public static IEventRouter? GetEventRoute(this IServiceProvider serviceProvider, string routeName)
    {
        if (!Routs.ContainsKey(routeName))
            return null;
            
        var eventRouterType = Routs[routeName];
        return serviceProvider.GetService(eventRouterType) as IEventRouter;
    }
}