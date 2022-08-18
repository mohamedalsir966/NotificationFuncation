using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using NotificationFunction.Service;

namespace NotificationFunction
{
    public class NotificationFunction
    {
        private readonly IService _serviceEngine;

        public NotificationFunction(IService serviceEngin)
        {
            _serviceEngine = serviceEngin;
        }
        [FunctionName("NotificationFunction")]
        public async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var Respose = await _serviceEngine.getupcomingshift();
            if(Respose != "NodataToBeNotify") _serviceEngine.SendDataToQueue(Respose);
           
        }
    }
}
