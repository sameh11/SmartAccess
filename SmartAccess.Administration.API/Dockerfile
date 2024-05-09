#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 7000
EXPOSE 7001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SmartAccess.Administration.API/SmartAccess.Administration.API.csproj", "SmartAccess.Administration.API/"]
RUN dotnet restore "./SmartAccess.Administration.API/SmartAccess.Administration.API.csproj"
COPY . .
WORKDIR "/src/SmartAccess.Administration.API"
RUN dotnet build "./SmartAccess.Administration.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SmartAccess.Administration.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartAccess.Administration.API.dll"]