#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["Webshop.Help/Webshop.Help.csproj", "Webshop.Help/"]
RUN dotnet restore "Webshop.Help/Webshop.Help.csproj"
COPY . .
# Ensure SQL seed files are available to MSBuild during build/publish
COPY ["Customers.sql", "Webshop.Help/"]
COPY ["Seed Demo Customers.sql", "Webshop.Help/"]
WORKDIR "/src/Webshop.Help"
RUN dotnet build "Webshop.Help.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "Webshop.Help.csproj" -c Debug -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Webshop.Help.dll"]