using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Review.Application.Contracts.Persistence;

namespace Webshop.Review.Persistence
{
    public static class ReviewInfrastructureRegistration
    {
        public static IServiceCollection AddReviewInfrastructureServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());            
            services.AddScoped<IReviewRepository, ReviewRepository>();

            return services;
        }
    }
}
