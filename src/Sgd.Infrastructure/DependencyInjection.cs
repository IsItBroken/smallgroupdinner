using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Polly;
using Polly.Extensions.Http;
using Sgd.Application.Common.Interfaces;
using Sgd.Infrastructure.CIAM.Auth0;
using Sgd.Infrastructure.Persistence;
using Sgd.Infrastructure.Persistence.Configurations;
using Sgd.Infrastructure.Persistence.Repositories;

namespace Sgd.Infrastructure;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        return builder
            .AddConfigurations()
            .AddCache()
            .AddAuth()
            .AddMediatR()
            .AddBackgroundServices()
            .AddPersistence()
            .AddServices();
    }

    private static WebApplicationBuilder AddConfigurations(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions();

        return builder;
    }

    private static WebApplicationBuilder AddCache(this WebApplicationBuilder builder)
    {
        builder.Services.AddMemoryCache();
        return builder;
    }

    private static WebApplicationBuilder AddAuth(this WebApplicationBuilder builder)
    {
        builder.AddAuth0Authentication();

        builder.Services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });

        return builder;
    }

    private static WebApplicationBuilder AddMediatR(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(options =>
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection))
        );

        return builder;
    }

    private static WebApplicationBuilder AddBackgroundServices(this WebApplicationBuilder builder)
    {
        return builder;
    }

    private static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<SgdDbOptions>(builder.Configuration.GetSection("SgdDatabase"));

        SgdDbModelConfiguration.RegisterSmartEnumSerializers();
        SgdDbModelConfiguration.ConfigureModel();

        builder.Services.AddSingleton<IMongoClient, MongoClient>(serviceProvider =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<SgdDbOptions>>().Value;
            return new MongoClient(settings.ConnectionString);
        });

        builder.Services.AddScoped<SgdDbContext>();
        builder.Services.AddScoped<IUnitOfWork, SgdDbContext>();

        builder.Services.AddScoped<IDinnerRepository, DinnerRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        return builder;
    }

    private static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
    {
        return builder;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
