# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0-noble AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0-noble AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Nuget.config", "."]
COPY ["src/server/BG/BG.API/BG.API.csproj", "src/server/BG/BG.API/"]
COPY ["src/frontend/frontend.esproj", "src/frontend/"]
COPY ["src/server/BG/BG.Application/BG.Application.csproj", "src/server/BG/BG.Application/"]
COPY ["src/server/Common/Common.Utils/Common.Utils.csproj", "src/server/Common/Common.Utils/"]
COPY ["src/server/BG/BG.Infrastructure/BG.Infrastructure.csproj", "src/server/BG/BG.Infrastructure/"]
COPY ["src/server/BG/BG.Core/BG.Core.csproj", "src/server/BG/BG.Core/"]
RUN dotnet restore "./src/server/BG/BG.API/BG.API.csproj"
COPY . .
WORKDIR "/src/src/server/BG/BG.API"
RUN dotnet build "./BG.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BG.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BG.API.dll"]