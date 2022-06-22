# Webshop Solution
[![Build](https://github.com/brhvucn/PSU_WebshopProductCatalog/actions/workflows/dotnet.yml/badge.svg)](https://github.com/brhvucn/PSU_WebshopProductCatalog/actions/workflows/dotnet.yml)

This solution contains several microservices that builds and mimics a microservice architecture for a web shop. The solution has been containerized to help set up everything in an easy way using docker.

The solution is a .NET 6.0 solution that is implemented following various principles, most services are built using a Clean Architecture and CQRS approach.

## Webshop Payment API
This is an API that resembles a payment api for a webshop it has some business rules and will simulate a payment. The docker image may be found in the `docker-compose.yml` file. The documentation is available using Swagger. In developent this is available at the root of the service `/` running docker this needs to be specified as `/swagger`.

## Webshop Customer API
This is an API that resembles a customer database. The service will take and store customers and has basic CRUD functionality. The documentation is available using Swagger. In developent this is available at the root of the service `/` running docker this needs to be specified as `/swagger`.

## Webshop Catalog API
This is an API that resembles a product catalog with products and categories. The documentation is available using Swagger. In developent this is available at the root of the service `/` running docker this needs to be specified as `/swagger`.

## Additional Ressources
As of right now the Customer and the Catalog API services require a database (MSSQL). The scripts are available in this solution and the `appsettings.json` controls the connection string. Please note that this is not tested on Docker.

## Docker and containers
The full solution can be started by using the enclosed `docker-compose.yml` file. In addition to the webshop related API two additional containers are started:
* Seq - The seq from datalust is a logging platform that the API services uses to log all exceptions and errors.
* Smtp4Dev - This service allows for easy access to a smtp mail server for development purpose. 
