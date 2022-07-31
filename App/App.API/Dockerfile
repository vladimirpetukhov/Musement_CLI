#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["App.API/App.API.csproj", "App.API/"]
COPY ["App.Services/App.Services.csproj", "App.Services/"]
COPY ["App.Core/App.Core.csproj", "App.Core/"]
COPY ["App.Infrastrucure/App.Infrastrucure.csproj", "App.Infrastrucure/"]
RUN dotnet restore "App.API/App.API.csproj"
COPY . .
WORKDIR "/src/App.API"
RUN dotnet build "App.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "App.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "App.API.dll"]