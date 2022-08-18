using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NotificationFunction.Service.Mapper
{
    public static class DependencyInjection
    {
        public static void AddServiceLayer(this IServiceCollection services)
        {
            
            var config = new MapperConfiguration(c => {
                c.AddProfile<LogsProfile>();
              
            });
            services.AddSingleton<IMapper>(s => config.CreateMapper());


        }
    }
}
