using Auth0Net.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Sgd.Application.Common.Interfaces;
using Sgd.Infrastructure.Authorization;

namespace Sgd.Infrastructure.CIAM.Auth0;

public static class Auth0DependencyInjection
{
    public static WebApplicationBuilder AddAuth0Authentication(this WebApplicationBuilder builder)
    {
        builder.ValidateRequiredConfigurationSet(
            [
                "Authentication:Auth0:Authority",
                "Authentication:Auth0:Audience",
                "Authentication:Auth0:ClientId",
                "Authentication:Auth0:ClientSecret"
            ]
        );

        builder.ValidateRequiredConfigurationSecretSet(["Authentication:Auth0:ClientSecret"]);

        builder
            .Services.AddOptions<Auth0Configuration>()
            .Bind(builder.Configuration.GetSection("Authentication:Auth0"));

        builder
            .Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = builder.Configuration["Authentication:Auth0:Authority"];
                options.Audience = builder.Configuration["Authentication:Auth0:Audience"];

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true
                };
            });

        builder.Services.AddAuth0AuthenticationClient(config =>
        {
            config.Domain =
                builder.Configuration.GetValue<string>("Authentication:Auth0:ManagementApiDomain")
                ?? throw new Exception("Authentication:Auth0:ManagementApiDomain is missing");
            config.ClientId = builder.Configuration.GetValue<string>(
                "Authentication:Auth0:ClientId"
            );
            config.ClientSecret = builder.Configuration.GetValue<string>(
                "Authentication:Auth0:ClientSecret"
            );
        });

        builder.Services.AddSingleton<IAuthorizationHandler, RbacHandler>();
        builder.Services.AddHttpClient<ITokenService, Auth0TokenService>();
        builder.Services.AddScoped<ICurrentUserProvider, GetCurrentUserForAuth0>();

        return builder;
    }
}
