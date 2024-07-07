#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS with-node
RUN apt-get update
RUN apt-get install curl
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash
RUN apt-get -y install nodejs
RUN npm install -g @angular/cli

FROM with-node AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["globalazure.netaspire.client/nuget.config", "globalazure.netaspire.client/"]
COPY ["GlobalAzure.NetAspire.Server/GlobalAzure.NetAspire.Server.csproj", "GlobalAzure.NetAspire.Server/"]
COPY ["GlobalAzure.NetAspire.Api.Contracts/GlobalAzure.NetAspire.Api.Contracts.csproj", "GlobalAzure.NetAspire.Api.Contracts/"]
COPY ["globalazure.netaspire.client/globalazure.netaspire.client.esproj", "globalazure.netaspire.client/"]
COPY ["GlobalAzure.NetAspire.ServiceDefaults/GlobalAzure.NetAspire.ServiceDefaults.csproj", "GlobalAzure.NetAspire.ServiceDefaults/"]
RUN dotnet restore "./GlobalAzure.NetAspire.Server/GlobalAzure.NetAspire.Server.csproj"
COPY . .
WORKDIR "/src/GlobalAzure.NetAspire.Server"
RUN dotnet build "./GlobalAzure.NetAspire.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GlobalAzure.NetAspire.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GlobalAzure.NetAspire.Server.dll"]