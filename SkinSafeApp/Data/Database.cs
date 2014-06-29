using System;
using SQLite;

namespace SkinSafeApp.Data
{
    public class Database
    {
        private readonly static Lazy<Database> _instance = new Lazy<Database>(() => new Database());
        private readonly SQLiteConnection _sqlConnection;

        public static Database Instance
        {
            get { return _instance.Value; }
        }

        private Database()
        {
            string folder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);

            _sqlConnection = new SQLiteConnection (System.IO.Path.Combine (folder, "database.db"));
            _sqlConnection.CreateTable<Area>();
            _sqlConnection.CreateTable<Sample>();
        }

        public void Insert(object o)
        {
            _sqlConnection.Insert(o);
        }

        public void Update(object o)
        {
            _sqlConnection.Update(o);
        }

        public void Delete(object o)
        {
            _sqlConnection.Delete(o);
        }

        public TableQuery<Sample> GetSamples()
        {
            return _sqlConnection.Table<Sample>();
        }

        public TableQuery<Area> GetAreas()
        {
            return _sqlConnection.Table<Area>();
        }
    }
}

