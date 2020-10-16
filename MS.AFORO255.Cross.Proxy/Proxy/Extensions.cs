using Microsoft.Extensions.DependencyInjection;

namespace MS.AFORO255.Cross.Proxy.Proxy
{
    public static class Extensions
    {
        public static IServiceCollection AddProxyHttp(this IServiceCollection services)
        {
            services.AddSingleton<IHttpClient, CustomHttpClient>();
            return services;
        }
    }
}
