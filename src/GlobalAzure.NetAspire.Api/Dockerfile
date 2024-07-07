#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GlobalAzure.NetAspire.Api/GlobalAzure.NetAspire.Api.csproj", "GlobalAzure.NetAspire.Api/"]
COPY ["GlobalAzure.NetAspire.Api.Contracts/GlobalAzure.NetAspire.Api.Contracts.csproj", "GlobalAzure.NetAspire.Api.Contracts/"]
COPY ["GlobalAzure.NetAspire.ServiceDefaults/GlobalAzure.NetAspire.ServiceDefaults.csproj", "GlobalAzure.NetAspire.ServiceDefaults/"]
RUN dotnet restore "./GlobalAzure.NetAspire.Api/GlobalAzure.NetAspire.Api.csproj"
COPY . .
WORKDIR "/src/GlobalAzure.NetAspire.Api"
RUN dotnet build "./GlobalAzure.NetAspire.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GlobalAzure.NetAspire.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GlobalAzure.NetAspire.Api.dll"]