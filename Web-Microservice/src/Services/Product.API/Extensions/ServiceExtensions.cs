
using Contracts.Commons.Interfaces;
using Contracts.Identity;
using Infrastructure.Commons;
using Infrastructure.Extensions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Product.API.Persistence;
using Product.API.Repositories;
using Product.API.Repositories.Interfaces;
using Shared.Configurations;
using System.Text;

namespace Product.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.ConfigureProductDbContext(configuration);
            services.AddInfrastructureService();
            services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));
            services.AddJwtAuthentication();
            return services;
        }

        private static IServiceCollection ConfigureProductDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnectionString");
            var builder = new MySqlConnectionStringBuilder(connectionString);

            services.AddDbContext<ProductContext>(x => x.UseMySql(builder.ConnectionString,
                ServerVersion.AutoDetect(builder.ConnectionString), e =>
            {
                e.MigrationsAssembly("Product.API");
                e.SchemaBehavior(MySqlSchemaBehavior.Ignore);
            }));
            return services;
        }

        private static IServiceCollection AddInfrastructureService(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IRepositoryBaseAsync<,,>), typeof(RepositoryBaseAsync<,,>))
                .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
                .AddScoped<IProductRepository, ProductRepository>()
                .AddTransient<ITokenService, TokenService>()
                ;
        }

        internal static IServiceCollection AddConfigurationSettings(this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection(nameof(JwtSettings))
                .Get<JwtSettings>();
            services.AddSingleton(jwtSettings);
            return services;
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
