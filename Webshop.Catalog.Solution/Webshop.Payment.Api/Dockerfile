#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Webshop.Payment.Api/Webshop.Payment.Api.csproj", "Webshop.Payment.Api/"]
RUN dotnet restore "Webshop.Payment.Api/Webshop.Payment.Api.csproj"
COPY . .
WORKDIR "/src/Webshop.Payment.Api"
RUN dotnet build "Webshop.Payment.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Webshop.Payment.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Webshop.Payment.Api.dll"]