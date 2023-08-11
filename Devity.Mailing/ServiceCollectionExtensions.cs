using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

namespace Devity.Mailing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDevityMailing<T>(this IServiceCollection services, ConfigurationManager configuration) where T : CommonMailService
    {
        return services
            .AddScoped<T>()
            .AddMailKit(options => options.UseMailKit(configuration.GetSection("Email").Get<MailKitOptions>()));
    }
}