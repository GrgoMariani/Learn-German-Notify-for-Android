using Notify.Interfaces;
using Notify.Models;
using SQLite;
using Xamarin.Forms;

namespace Notify.Data
{
    public class SettingsDatabaseController
    {
        const int SETTINGS_ID = 1;
        const string DatabaseName = "settings.db3";

        static readonly object locker = new object();

        readonly SQLiteConnection connection;

        public SettingsDatabaseController()
        {
            var sqliteController = DependencyService.Get<ISQLite>();
            var dbPath = sqliteController.GetPlatformDBPath(DatabaseName);
            connection = sqliteController.GetConnection(dbPath);
            connection.CreateTable<ItemSettings>();
        }

        public ItemSettings GetSettings()
        {
            lock (locker)
            {
                if (HasSettings())
                {
                    return connection.Get<ItemSettings>(SETTINGS_ID);
                }
                else
                {
                    return null;
                }
            }
        }

        public int SaveSettings(ItemSettings settings)
        {
            lock (locker)
            {
                if (settings.ID.HasValue)
                {
                    connection.Update(settings);
                    return settings.ID.Value;
                }
                else
                {
                    settings.ID = SETTINGS_ID;
                    return connection.Insert(settings);
                }
            }
        }

        public int DeleteSettings()
        {
            lock (locker)
            {
                return connection.Delete<ItemSettings>(SETTINGS_ID);
            }
        }

        public bool HasSettings()
        {
            return (connection.Table<ItemSettings>().Where(p => p.ID == SETTINGS_ID).Count() == 1);
        }
    }
}
