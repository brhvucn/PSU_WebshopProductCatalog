#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Webshop.Catalog.Api/Webshop.Catalog.Api.csproj", "Webshop.Catalog.Api/"]
COPY ["Webshop.Catalog.Application/Webshop.Catalog.Application.csproj", "Webshop.Catalog.Application/"]
COPY ["Webshop.Application/Webshop.Application.csproj", "Webshop.Application/"]
COPY ["Webshop.Domain/Webshop.Domain.csproj", "Webshop.Domain/"]
COPY ["Webshop.Catalog.Domain/Webshop.Catalog.Domain.csproj", "Webshop.Catalog.Domain/"]
COPY ["Webshop.Catalog.Persistence/Webshop.Catalog.Persistence.csproj", "Webshop.Catalog.Persistence/"]
COPY ["Webshop.Data.Persistence/Webshop.Data.Persistence.csproj", "Webshop.Data.Persistence/"]
RUN dotnet restore "Webshop.Catalog.Api/Webshop.Catalog.Api.csproj"
COPY . .
WORKDIR "/src/Webshop.Catalog.Api"
RUN dotnet build "Webshop.Catalog.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Webshop.Catalog.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Webshop.Catalog.Api.dll"]