using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RestSharp;
using SharedKernel.WebSockets;
using TradeAppApplication;
using TradeAppApplication.Configuration.Mappings;
using TradeAppDataAccess;
using TradeAppSharedKernel.Application;
using TradeAppSharedKernel.ExternalAPI;
using TradeAppSharedKernel.ExternalApiService;
using TradeAppSharedKernel.Infrastructure.Helpers.Token;
using TradeAppSharedKernel.Infrastructure.Helpers.TokenInfo;
using TradeAppSharedKernel.Infrastructure.Helpers.TokenService;
using TradeAppSharedKernel.Infrastructure.Security;

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
            services.AddScoped<ITokenService, TokenService>();
            services.AddSingleton<WebSocketConnectionManager>();
            services.AddSingleton<IWebSocketHandler, WebSocketHandler>();
            services.Configure<BaseApiService>(configuration.GetSection("ExternalApi"));

            services.AddHttpClient<IExternalApiService, ExternalApiService>((sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<BaseApiService>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
            });

            services.AddSingleton<RestClient>(sp =>
            {
                var options = sp.GetRequiredService<IOptions<BaseApiService>>().Value;
                return new RestClient(options.BaseUrl);
            });


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
