using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationFunction;
using NotificationFunction.Service;
using Persistence.Repositories;
using Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;
using NotificationFunction.Service.Mapper;

[assembly: FunctionsStartup(typeof(Startup))]

namespace NotificationFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddScoped<IService, ServiceEng>();
            builder.Services.AddServiceLayer();
            builder.Services.AddScoped<ILogsRepository, LogsRepository>();
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("Data Source=.;Initial Catalog=NotificationDB;Integrated Security=True"));
            
        }
    }
}
