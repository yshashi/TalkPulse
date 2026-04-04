using System.Reflection;

namespace TalkPulse.Api.Common.Extensions;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}
public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var endpointTypes = assembly.GetTypes()
            .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var type in endpointTypes)
        {
            services.AddTransient(typeof(IEndpoint), type);
        }

        return services;
    }

    public static IApplicationBuilder MapEndPoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        var endpoints = app.Services.GetServices<IEndpoint>();

        foreach (var endpoint in endpoints)
        {
            if (routeGroupBuilder is not null)
            {
                endpoint.MapEndpoint(routeGroupBuilder);
            }
            else
            {
                endpoint.MapEndpoint(app);
            }
        }

        return app;
    }
}