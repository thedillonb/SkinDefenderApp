using System;
using SQLite;

namespace SkinSafeApp.Data
{
    public class Area
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Location { get; set; }
    }
}

