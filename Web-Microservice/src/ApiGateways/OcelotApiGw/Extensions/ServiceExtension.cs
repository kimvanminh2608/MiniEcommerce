using Contracts.Identity;
using Infrastructure.Extensions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Shared.Configurations;
using System.Text;

namespace OcelotApiGw.Extensions
{
    public static class ServiceExtension
    {
        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            //var eventBusSetting = configuration.GetSection(nameof(EventBusSettings))
            //                    .Get<EventBusSettings>();

            //services.AddSingleton(eventBusSetting);

            //var cacheSetting = configuration.GetSection(nameof(CacheSettings))
            //                    .Get<CacheSettings>();

            //services.AddSingleton(cacheSetting);

            //var grpcSetting = configuration.GetSection(nameof(GrpcSetting))
            //                    .Get<GrpcSetting>();

            //services.AddSingleton(grpcSetting);
            var jwtSettings = configuration.GetSection(nameof(JwtSettings))
                .Get<JwtSettings>();
            services.AddSingleton(jwtSettings);
            return services;
        }

        public static void ConfigureOcelot(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOcelot(configuration);
            services.AddTransient<ITokenService, TokenService>();
            services.AddJwtAuthentication();
        }

        public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            var origins = configuration["AllowOrigins"];
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins(origins)
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
        }

        internal static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            var settings = services.GetOptions<JwtSettings>(nameof(JwtSettings));
            if (settings == null)
                throw new ArgumentNullException($"{nameof(settings)} is not configured property!");

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key));
            var tokenValidationParam = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = false,
            };
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.RequireHttpsMetadata = false;
                x.TokenValidationParameters = tokenValidationParam;
            });
            return services;
        }
    }
}
