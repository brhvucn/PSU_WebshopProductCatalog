﻿using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application;
using Webshop.Application.Contracts;

namespace Webshop.Catalog.Application
{
    public static class CatalogApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddApplicationServices(); //register the general services from webshop.applicaiton
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<IDispatcher>(sp => new Dispatcher(sp.GetService<IMediator>()));
            return services;
        }
    }
}
