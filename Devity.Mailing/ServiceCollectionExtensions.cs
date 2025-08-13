using Devity.NETCore.MailKit.Extensions;
using Devity.NETCore.MailKit.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Devity.Mailing;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDevityMailing<T>(
        this IServiceCollection services,
        ConfigurationManager configuration
    )
        where T : CommonMailService
    {
        var emailSection = configuration.GetSection("Email");

        if (!emailSection.Exists())
            throw new Exception("Missing Email section in the appsettings.");

        var emailOptions = emailSection.Get<MailKitOptions>();

        if (emailOptions is null)
            throw new Exception(
                "The Email section in the appsettings should be in a MailKitOptions format."
            );

        return services.AddScoped<T>().AddMailKit(options => options.UseMailKit(emailOptions));
    }

    public static IServiceCollection AddDevityMailing<T>(
        this IServiceCollection services,
        MailKitOptions mailKitOptions
    )
        where T : CommonMailService =>
        services.AddScoped<T>().AddMailKit(options => options.UseMailKit(mailKitOptions));
}
