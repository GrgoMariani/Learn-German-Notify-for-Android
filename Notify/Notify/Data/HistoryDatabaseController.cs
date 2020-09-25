using Notify.Interfaces;
using Notify.Models;
using SQLite;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace Notify.Data
{
    /// <summary>
    /// This Database will be used as read-only.
    /// It will first be copied from assets and unzipped on AppStart using the CopyDBFromAssets() method.
    /// </summary>
    public class HistoryDatabaseController
    {
        const int MAX_NO_OF_RESULTS = 200;

        const string DatabaseName = "history.db3";

        readonly SQLiteConnection connection;

        static readonly object locker = new object();

        public HistoryDatabaseController()
        {
            var sqliteController = DependencyService.Get<ISQLite>();
            var dbPath = sqliteController.GetPlatformDBPath(DatabaseName);
            connection = sqliteController.GetConnection(dbPath);

            connection.CreateTable<ItemTranslation>();
        }

        public void SaveToHistory(ItemTranslation itemTranslation)
        {
            lock(locker)
            {
                // Save this item to history
                connection.Insert(itemTranslation);
            }
            
        }

        public IEnumerable<ItemTranslation> GetLastTranslations()
        {
            lock (locker)
            {
                var result = connection.Query<ItemTranslation>($"SELECT * FROM ItemTranslation ORDER BY id DESC; LIMIT {MAX_NO_OF_RESULTS}");
                // handle delete
                return result;
            }
        }
    }
}
