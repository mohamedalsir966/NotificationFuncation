using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NotificationFunction.Entitys;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.Bot.Builder;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Persistence.Repositories;
using Domain;
using System.Buffers.Text;
using System.Globalization;
using AutoMapper;

namespace NotificationFunction.Service
{
    public class ServiceEng : IService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogsRepository _LogsRepository;
        private readonly IMapper _mapper;

        public ServiceEng(IMapper mapper,IConfiguration configuration, ILogsRepository logsRepository)
        {
            _mapper = mapper;
            _configuration = configuration;
            _LogsRepository = logsRepository;
        }
        public async Task<string> getupcomingshift()
        {
            var ShiftiInterval = _configuration.GetValue<int>("ShiftiInterval");

            #region to test local
            //for test local
            DateTime currentDateTime = DateTime.Parse("2022-08-09T08:40:00",
                 CultureInfo.InvariantCulture,
                 DateTimeStyles.AdjustToUniversal);
            #endregion
            // var currentDateTime = DateTime.UtcNow;
            var UpperInterval = currentDateTime.AddMinutes(ShiftiInterval);
            var TodayShifts = UpperInterval.Date;
            var MyShiftResponse = await GetDataFromShiftApi(TodayShifts);
            
            var DataToBeNotify = JsonConvert.DeserializeObject<PaginatedList<ShiftEntity>>(MyShiftResponse);
            var IntenminutesNotifications = DataToBeNotify.Items.
                Where(x => x.StartDateTime.TimeOfDay.TotalMinutes < UpperInterval.TimeOfDay.TotalMinutes
                         && x.EndDateTime.TimeOfDay.TotalMinutes >= currentDateTime.TimeOfDay.TotalMinutes);

            //check if the data empty
            if (IntenminutesNotifications.Count() <= 0) return "NodataToBeNotify";
            var maplist = IntenminutesNotifications.ToList();
            var tocash1 = _mapper.Map<List<ShiftEntityLog>>(maplist);
            var listofid = tocash1.Select(x=>x.Id).ToList();
           var ExistingIdsLogs = await _LogsRepository.GetLogsQueries(listofid);

            #region comparing the two list
            HashSet<Guid> diffids = new HashSet<Guid>(ExistingIdsLogs.Select(s => s.Id));
            var results = tocash1.Where(m => !diffids.Contains(m.Id)).ToList();
            #endregion
            if (results.Count <= 0) return "NodataToBeNotify";
            await _LogsRepository.CreateNewLogsCommand(results);
            return MyShiftResponse;
        }
        public bool SendDataToQueue(string message)
        {
            try
            {
                string connectionString = _configuration.GetValue<string>("AzureWebJobsStorage");
                QueueClient queueClient = new QueueClient
                (connectionString, "test3");
                queueClient.CreateIfNotExists();
                if (queueClient.Exists())
                {
                    queueClient.SendMessage(Convert.ToBase64String(Encoding.UTF8.GetBytes(message)));
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<string> GetDataFromShiftApi(DateTime TodayShifts)
        {
            HttpClient httpClient = new HttpClient();
            var BaseUrl = _configuration.GetValue<string>("BaseUrl");
            var RequstUrl = BaseUrl + "?StartDate=" + TodayShifts;
            var MyShiftResponse = await httpClient.GetStringAsync(RequstUrl);
            return MyShiftResponse;
        }
       
    }
}
