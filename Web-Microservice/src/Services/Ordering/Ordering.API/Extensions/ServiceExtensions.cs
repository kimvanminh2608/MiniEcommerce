using Infrastructure.Configuation;

namespace Ordering.API.Extensions
{
    public static class ServiceExtensions
    {
        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSetting = configuration.GetSection(nameof(SMTPEmailSetting))
                                .Get<SMTPEmailSetting>();

            services.AddSingleton(emailSetting);
            return services;
        }

    }
}
