using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace AppNotas.Models
{
    [Table("Note")]
    public class Note : INamable, IOrderable, INamableAndOrderable
    {
        public Note() {  }

        public Note(string _name)  
        {
            this.name = _name;
            this.text = ""; 
            this.SectionId = null;
            this.order = 0;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string name { get; set; }

        public int order { get; set; }

        [MaxLength(2147483647)]
        public string text { get; set; }

        [ForeignKey(typeof(Section))]
        public int? SectionId { get; set; }

        public int? progress { get; set; }
    }
}

