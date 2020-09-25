using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ICSharpCode.SharpZipLib.Zip;
using Notify.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(Notify.Droid.Interfaces.AndroidSQLite))]
namespace Notify.Droid.Interfaces
{
    /// <summary>
    /// https://forums.xamarin.com/discussion/176299/local-sqlite-database-in-xamarin-forms
    /// </summary>
    public class AndroidSQLite : ISQLite
    {
        public AndroidSQLite() { }

        public SQLite.SQLiteConnection GetConnection(string dbname)
        {
            var path = GetPlatformDBPath(dbname);
            var conn = new SQLite.SQLiteConnection(path);

            return conn;
        }

        public string GetPlatformDBPath(string fName)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, fName);
        }
    }
}