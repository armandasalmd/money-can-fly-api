using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MCF.Core;

public static class Extensions
{
    public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
    {
        builder.Services.AddServiceDiscovery();
        return builder;
    }
}