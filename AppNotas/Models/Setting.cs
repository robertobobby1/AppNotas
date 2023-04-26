using System;
using SQLite;

namespace AppNotas.Models
{
	public class Setting
	{
		public Setting() { }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string key { get; set; }
        public int value { get; set; }
    }
}

