using RecordsTable.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecordsTable.Services
{
    public class ScheduleGenerationService
    {
        private DateTime GetToDateTimeOfRecord(Record record) => record.DateTime.AddSeconds(record.SeanceLength);

        public List<ScheduleRecord> Create(List<Record> data, DateTime from, DateTime to)
        {
            if (data == null || data.Count() == 0)
            {
                return new List<ScheduleRecord>()
                {
                    new ScheduleRecord() { From = from, To = to },
                };
            }

            if (data.Any(a => a.DateTime < from || GetToDateTimeOfRecord(a) > to))
            {
                var b = data.FindAll(a => a.DateTime < from || GetToDateTimeOfRecord(a) > to);
                throw new ArgumentException("Записи не могут выходить за диапазон расписания");
            }

            List<Record> sortedData = data.OrderBy(a => a.DateTime).ToList();
            int firstRecordIndex = 0;
            Record firstRecord = sortedData.First();
            int lastRecordIndex = sortedData.Count() - 1;
            Record lastRecord = sortedData.Last();

            List<ScheduleRecord> result = new List<ScheduleRecord>();

            if (firstRecordIndex == lastRecordIndex)
            {
                if (firstRecord.DateTime != from)
                {
                    result.Add(new ScheduleRecord()
                    {
                        From = from,
                        To = firstRecord.DateTime,
                    });
                }

                result.Add(new ScheduleRecord()
                {
                    From = firstRecord.DateTime,
                    To = GetToDateTimeOfRecord(firstRecord),
                    Record = firstRecord,
                });

                if (GetToDateTimeOfRecord(lastRecord) != to)
                {
                    result.Add(new ScheduleRecord()
                    {
                        From = GetToDateTimeOfRecord(lastRecord),
                        To = to,
                    });
                }

                return result;
            }

            if (firstRecord.DateTime != from)
            {
                result.Add(new ScheduleRecord()
                {
                    From = from,
                    To = firstRecord.DateTime,
                });
            }

            result.Add(new ScheduleRecord()
            {
                From = firstRecord.DateTime,
                To = GetToDateTimeOfRecord(firstRecord),
                Record = firstRecord,
            });

            for (int i = firstRecordIndex + 1; i <= lastRecordIndex; i++)
            {
                Record previousRecord = sortedData[i - 1];

                if (sortedData[i].DateTime != GetToDateTimeOfRecord(previousRecord))
                {
                    result.Add(new ScheduleRecord()
                    {
                        From = GetToDateTimeOfRecord(previousRecord),
                        To = sortedData[i].DateTime,
                    });
                }

                result.Add(new ScheduleRecord()
                {
                    From = sortedData[i].DateTime,
                    To = GetToDateTimeOfRecord(sortedData[i]),
                    Record = sortedData[i],
                });
            }

            if (GetToDateTimeOfRecord(lastRecord) != to)
            {
                result.Add(new ScheduleRecord()
                {
                    From = GetToDateTimeOfRecord(lastRecord),
                    To = to,
                });
            }

            return result;
        }
    }
}
