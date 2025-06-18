using Microsoft.EntityFrameworkCore;
using TradeAppApplication.Configuration.Mappings;
using TradeAppApplication;
using TradeAppDataAccess;
using TradeAppSharedKernel.Application;
using TradeAppSharedKernel.Infrastructure.Security;
using TradeAppSharedKernel.Infrastructure.Helpers.Token;
using TradeAppSharedKernel.Infrastructure.Helpers.TokenInfo;

namespace TradeAppAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<ITokenInfoHandler, TokenInfoHandler>();

            return services;
        }

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                    .WithOrigins("http://localhost:3001")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
