using AutoMapper;
using Domain;
using Newtonsoft.Json.Linq;
using NotificationFunction.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationFunction.Service.Mapper
{
    public class LogsProfile : Profile
    {
        public LogsProfile()
        {
            CreateMap<ShiftEntityLog, ShiftEntity>();
            CreateMap<ShiftEntity, ShiftEntityLog>();
            CreateMap<ShiftEntity, ShiftEntityLog>()
            .BeforeMap((s, d) => d.status = "not sent");

        }
                   
    }
}
