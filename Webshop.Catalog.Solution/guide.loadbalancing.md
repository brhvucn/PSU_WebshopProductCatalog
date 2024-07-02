# Guide - load balancing

This docker compose file will start two instances of the same service inside a docker container. The load balancer will be started in your local maching and distribute the requests between the two instances of the services.

## Getting started
There are a couple of steps to get started with the sample:
1: Start the services by executing the following command: `docker-compose up` - this will start the two services that we want to load balance. Inspecting the docker-compose file, you will see that we are starting two instances of the same service. Please note that we are giving the services an instance name through the environment variable: "INSTANCE_ID" which we later are getting in the `LicenseHelper` class.
2: Start the load balancer

## Known Issues
* Certificates - when running in development mode, the services will not have a valid certificate. This will cause the load balancer to not work properly. To fix this, you can either disable the certificate validation in the services or create a valid certificate for the services.
* This sample is configured in the ocelot configuration file to use the http protocol (to avoid having the vertificate issues). If you want to use https, you can change the configuration in the ocelot configuration file.
* When the loadbalancer starts it gives an error that there is no page listening for the root call. This is normal and you need to call the upstream endpoints directly