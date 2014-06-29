using System;
using SQLite;

namespace SkinSafeApp.Data
{
    public class Sample
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int AreaId { get; set; }

        public DateTime Date { get; set; }

        [MaxLength(256)]
        public string ImagePath { get; set; }

        [MaxLength(256)]
        public string ThumbnailPath { get; set; }
    }
}

