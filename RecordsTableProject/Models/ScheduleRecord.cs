using System;

namespace RecordsTable.Models
{
    public class ScheduleRecord
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public Record? Record { get; set; }
    }
}