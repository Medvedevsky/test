using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsTable.Models
{
    public class StaffSchedule
    {
        public List<ScheduleRecord> ScheduleRecords { get; set; }
        public Staff Staff { get; set; }
    }
}
