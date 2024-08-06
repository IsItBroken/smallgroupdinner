FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Sgd.Api/Sgd.Api.csproj", "Sgd.Api/"]
COPY ["Sgd.Application/Sgd.Application.csproj", "Sgd.Application/"]
COPY ["Sgd.Domain/Sgd.Domain.csproj", "Sgd.Domain/"]
COPY ["Sgd.Infrastructure/Sgd.Infrastructure.csproj", "Sgd.Infrastructure/"]

RUN dotnet restore "Sgd.Api/Sgd.Api.csproj"
COPY . .
WORKDIR "/src/Sgd.Api"
RUN dotnet build "Sgd.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Sgd.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sgd.Api.dll"]