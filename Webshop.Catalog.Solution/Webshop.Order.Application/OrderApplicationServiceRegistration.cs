using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application;
using Webshop.Application.Contracts;

namespace Webshop.Order.Application
{
    public static class OrderApplicationServiceRegistration
    {
        public static IServiceCollection AddCustomerApplicationServices(this IServiceCollection services)
        {
            services.AddApplicationServices();
            services.AddMediatR(Assembly.GetExecutingAssembly());
            //services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IDispatcher>(sp => new Dispatcher(sp.GetService<IMediator>()));

            return services;
        }
    }
}
