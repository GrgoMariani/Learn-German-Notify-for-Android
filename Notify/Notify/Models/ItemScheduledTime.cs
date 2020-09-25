using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notify.Models
{
    public class ItemScheduledTime
    {

        [PrimaryKey, AutoIncrement]
        public int? ID { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }

        public string TimeToString
        {
            get => $"{Hour:00}:{Minute:00}";
        }
    }
}
