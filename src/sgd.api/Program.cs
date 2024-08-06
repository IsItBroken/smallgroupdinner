using Sgd.Api;
using Sgd.Application;
using Sgd.Infrastructure;
using Sgd.Infrastructure.CIAM.Auth0;

var builder = WebApplication.CreateBuilder(args);


{
    builder.AddPresentation().AddApplication().AddInfrastructure();

    builder.Services.AddHealthChecks();

    builder.Services.AddCors(options =>
        options.AddPolicy(
            "AllowAll",
            p =>
                p.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("Content-Disposition")
        )
    );
}

var app = builder.Build();


{
    app.UseCors("AllowAll");
    app.UseExceptionHandler();
    if (app.Environment.IsDevelopment() || true)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Small Group Dinner API V1");
            c.RoutePrefix = string.Empty;
        });
    }

    app.UseHttpsRedirection();

    app.MapHealthChecks("/healthz");

    app.AddInfrastructureMiddleware();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseMiddleware<UserExistsMiddleware>();

    app.MapControllers();

    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    var url = $"http://0.0.0.0:{port}";
    app.Run(url);
}
