using SQLite;

namespace Notify.Interfaces
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection(string dbname);

        string GetPlatformDBPath(string filename);

    }
}
