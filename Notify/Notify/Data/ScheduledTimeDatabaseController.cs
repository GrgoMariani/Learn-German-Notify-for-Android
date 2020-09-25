using Notify.Interfaces;
using Notify.Models;
using SQLite;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Notify.Data
{
    public class ScheduledTimeDatabaseController
    {
        const string DatabaseName = "scheduledTimes.db3";

        static readonly object locker = new object();

        readonly SQLiteConnection connection;

        public ScheduledTimeDatabaseController()
        {
            var sqliteController = DependencyService.Get<ISQLite>();
            var dbPath = sqliteController.GetPlatformDBPath(DatabaseName);
            connection = sqliteController.GetConnection(dbPath);
            connection.CreateTable<ItemScheduledTime>();
        }

        public IEnumerable<ItemScheduledTime> GetAllScheduledTimes()
        {
            lock(locker)
            {
                return connection.Query<ItemScheduledTime>("SELECT * FROM ItemScheduledTime ORDER BY Hour ASC, Minute ASC;");
            }
        }

        public void SaveItems(IEnumerable<ItemScheduledTime> itemScheduledTimes)
        {
            foreach(var itemScheduledTime in itemScheduledTimes)
            {
                SaveScheduledTime(itemScheduledTime);
            }
        }

        public void SaveScheduledTime(ItemScheduledTime itemScheduledTime)
        {
            lock(locker)
            {
                connection.InsertOrReplace(itemScheduledTime);
            }
        }

        public void DeleteScheduledTime(ItemScheduledTime itemScheduledTime)
        {
            lock(locker)
            {
                if (itemScheduledTime.ID.HasValue)
                {
                    connection.Delete<ItemScheduledTime>(itemScheduledTime.ID);
                }
            }
        }
    }
}
