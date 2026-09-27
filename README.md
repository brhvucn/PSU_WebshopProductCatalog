# Webshop Solution
[![Build](https://github.com/brhvucn/PSU_WebshopProductCatalog/actions/workflows/dotnet.yml/badge.svg)](https://github.com/brhvucn/PSU_WebshopProductCatalog/actions/workflows/dotnet.yml)

This solution contains several microservices that builds and mimics a microservice architecture for a web shop. The solution has been containerized to help set up everything in an easy way using docker.

The solution is a **.NET 10** solution that is implemented following various principles, most services are built using a **Clean Architecture and CQRS** approach.

## Webshop Payment API
This is an API that resembles a payment api for a webshop it has some business rules and will simulate a payment. The docker image may be found in the `docker-compose.yml` file. The documentation is available using Swagger. In developent this is available at the root of the service `/` running docker this needs to be specified as `/swagger`.

## Webshop Customer API
This is an API that resembles a customer database. The service will take and store customers and has basic CRUD functionality. The documentation is available using Swagger. In developent this is available at the root of the service `/` running docker this needs to be specified as `/swagger`.

## Webshop Catalog API
This is an API that resembles a product catalog with products and categories. The documentation is available using Swagger. In developent this is available at the root of the service `/` running docker this needs to be specified as `/swagger`.

## Quick Start with Docker

**Prerequisites:**
- Docker Desktop installed and running
- Ports 8000-8087 and 1403 available

**Start all services:**
```powershell
cd Webshop.Catalog.Solution
docker-compose up
```

**Access the services:**
- **Catalog API:** http://localhost:8084/swagger
- **Customer API:** http://localhost:8085/swagger
- **Payment API:** http://localhost:8083/swagger
- **Review API:** http://localhost:8086/swagger
- **Help Service:** http://localhost:8000
- **Seq (Logs):** http://localhost:8081
- **Smtp4Dev (Email):** http://localhost:8082
- **Prometheus (Metrics):** http://localhost:8087

**Stop services:**
```powershell
docker-compose down
```

**Reset database:**
```powershell
docker-compose down -v
docker-compose up
```

## Additional Resources
The Customer and Catalog API services require a database (MSSQL). The database is automatically created in the Docker container. SQL seed scripts are available in the solution root.

## Infrastructure Services
In addition to the webshop APIs, the following infrastructure services are included:
* **Seq** - Centralized logging platform from Datalust for all API exceptions and errors
* **Smtp4Dev** - SMTP mail server for development and testing
* **Prometheus** - Metrics collection and monitoring
* **SQL Server** - Database server (MSSQL Express in Docker) 
