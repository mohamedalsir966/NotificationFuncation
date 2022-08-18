using NotificationFunction.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationFunction.Entitys
{
    public class ShiftEntity : BaseEntity
    {
        private DateTime _startDateTime;
        private DateTime _endDateTime;
        public Guid EmployeeId { get; set; }
        public DateTime StartDateTime
        {
            get
            {
                return _startDateTime;
            }
            set
            {
                _startDateTime = value.Truncate(TimeSpan.FromMinutes(1));
            }
        }
        public DateTime EndDateTime
        {
            get
            {
                return _endDateTime;
            }
            set
            {
                _endDateTime = value.Truncate(TimeSpan.FromMinutes(1));
            }
        }
        public Guid AreaId { get; set; }
        public Guid RoleId { get; set; }
        public Guid EventId { get; set; }
    }
}
