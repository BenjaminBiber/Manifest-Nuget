using BenjaminBiber.Manifest.Interfaces;
using BenjaminBiber.Manifest.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace BenjaminBiber.Manifest;

public static class ManifestServiceCollectionsExtensions
{
    public static IServiceCollection AddCms(
        this IServiceCollection services,
        string baseUrl,
        string apiPrefix = "/api/")
    {
        services.TryAddSingleton<ICacheService>(_ => new CacheService(TimeSpan.FromHours(1)));
        services.AddHttpClient<ICmsService, CmsService>(http =>
        {
            var root = new Uri(baseUrl.TrimEnd('/') + "/");
            http.BaseAddress = new Uri(root, apiPrefix.TrimStart('/'));
        });
        
        return services;
    }
}