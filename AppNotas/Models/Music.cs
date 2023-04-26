using System;
using SQLite;

namespace AppNotas.Models
{
    public class Music
    {
        public Music() { }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string title { get; set; }

        public string artist { get; set; }

        public string url { get; set; }

        public string coverImage { get; set; } = "https://usercontent2.hubstatic.com/14548043_f1024.jpg";

        public bool isRecent { get; set; }
    }
}

