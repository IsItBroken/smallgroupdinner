using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sgd.Application.Common.Behaviors;
using Sgd.Application.Groups.Services;

namespace Sgd.Application;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));

            options.AddOpenBehavior(typeof(LoggingBehaviour<,>));
            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        builder.Services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));

        builder.Services.AddScoped<GroupMemberService>();

        return builder;
    }
}
