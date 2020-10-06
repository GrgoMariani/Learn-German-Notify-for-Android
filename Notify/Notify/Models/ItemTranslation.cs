using SQLite;

namespace Notify.Models
{
    public class ItemTranslation
    {
        [PrimaryKey, AutoIncrement]
        public int? id { get; set; }
        public string German { get; set; }
        public string English { get; set; }
        public string GermanExample { get; set; }
        public string EnglishExample { get; set; }
        public string Difficulty { get; set; }
    }
}