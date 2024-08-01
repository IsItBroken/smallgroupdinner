using Sgd.Api;
using Sgd.Application;
using Sgd.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.AddPresentation().AddApplication().AddInfrastructure();

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
    app.AddInfrastructureMiddleware();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Small Group Dinner API V1");
            c.RoutePrefix = string.Empty; // Set the Swagger UI at the root
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}