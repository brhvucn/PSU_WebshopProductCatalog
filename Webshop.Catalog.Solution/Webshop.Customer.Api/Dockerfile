#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Webshop.Customer.Api/Webshop.Customer.Api.csproj", "Webshop.Customer.Api/"]
COPY ["Webshop.Customer.Application/Webshop.Customer.Application.csproj", "Webshop.Customer.Application/"]
COPY ["Webshop.Application/Webshop.Application.csproj", "Webshop.Application/"]
COPY ["Webshop.Domain/Webshop.Domain.csproj", "Webshop.Domain/"]
COPY ["Webshop.Customer.Persistence/Webshop.Customer.Persistence.csproj", "Webshop.Customer.Persistence/"]
COPY ["Webshop.Data.Persistence/Webshop.Data.Persistence.csproj", "Webshop.Data.Persistence/"]
RUN dotnet restore "Webshop.Customer.Api/Webshop.Customer.Api.csproj"
COPY . .
WORKDIR "/src/Webshop.Customer.Api"
RUN dotnet build "Webshop.Customer.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Webshop.Customer.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Webshop.Customer.Api.dll"]