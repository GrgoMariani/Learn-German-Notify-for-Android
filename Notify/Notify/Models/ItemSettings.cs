using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notify.Models
{
    public class ItemSettings
    {

        [PrimaryKey, AutoIncrement]
        public int? ID { get; set; }
        public bool IsDatabaseSetUp { get; set; }
    }
}
