using System;

namespace V8Media.Framework.Models.Custom
{
    public class ArchiveDate
    {
        public ArchiveDate(DateTime date,int count)
        {
            Date = date;
            NodeCount = count;
        }

        public DateTime Date { get; set; }
        public int NodeCount { get; set; }
    }
}