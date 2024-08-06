using Microsoft.OpenApi.Models;

namespace Sgd.Api;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Small Group Dinner API", Version = "v1" });
        });
        builder.Services.AddProblemDetails();
        builder.Services.AddHttpContextAccessor();

        return builder;
    }
}
