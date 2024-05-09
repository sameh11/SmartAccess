#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SmartAccess.Identity.API/SmartAccess.Identity.API.csproj", "SmartAccess.Identity.API/"]
RUN dotnet restore "SmartAccess.Identity.API/SmartAccess.Identity.API.csproj"
COPY . .
WORKDIR "/src/SmartAccess.Identity.API"
RUN dotnet build "SmartAccess.Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartAccess.Identity.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartAccess.Identity.API.dll"]