using RecordsTable.Models;
using RecordsTable.Services.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordsTable.Services
{
    internal class StaffScheduleGenerationService
    {
        private ScheduleGenerationService _schedule;
        public StaffScheduleGenerationService(ScheduleGenerationService schedule)
        {
            _schedule = schedule;
        }
        public List<StaffSchedule> CreateStaffRecords(List<Record> records, List<Staff> staffs, DateTime from, DateTime to)
        {

            List<StaffSchedule> staffSchedules = new List<StaffSchedule>();

            foreach(var staff in staffs)
            {
                IEnumerable<Record>? recordsByStaff = records.Where(a => a.Staff.Id == staff.Id);
                List<ScheduleRecord> scheduleRecords = _schedule.Create(recordsByStaff.ToList(), from, to);

                staffSchedules.Add(new StaffSchedule() { Staff = staff, ScheduleRecords = scheduleRecords });
            }

            return staffSchedules;
        }
    }
}
