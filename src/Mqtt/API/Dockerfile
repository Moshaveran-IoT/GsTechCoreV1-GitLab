#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Mqtt/API/API.csproj", "src/Mqtt/API/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Mqtt/Application/Application.csproj", "src/Mqtt/Application/"]
COPY ["src/Mqtt/DataAccess/DataAccess.csproj", "src/Mqtt/DataAccess/"]
COPY ["src/Mqtt/Domain/Domain.csproj", "src/Mqtt/Domain/"]
RUN dotnet restore "./src/Mqtt/API/API.csproj"
COPY . .
WORKDIR "/src/src/Mqtt/API"
RUN dotnet build "./API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Moshaveran.IoT.API.dll"]