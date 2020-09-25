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
    public class TranslationDatabaseController
    {
        const string DatabaseName = "translations.db3";

        readonly SQLiteConnection connection;

        public TranslationDatabaseController()
        {
            var sqliteController = DependencyService.Get<ISQLite>();
            var dbPath = sqliteController.GetPlatformDBPath(DatabaseName);
            connection = sqliteController.GetConnection(dbPath);
        }

        public ItemTranslation GetRandomTranslation()
        {
            IList<ItemTranslation> query = connection.Query<ItemTranslation>("SELECT * FROM ItemTranslation ORDER BY RANDOM() LIMIT 1");
            ItemTranslation result = query.First();
            return result;
        }

    }
}
