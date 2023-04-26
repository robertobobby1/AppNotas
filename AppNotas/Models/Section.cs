using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace AppNotas.Models
{
	public class Section : INamable, IOrderable, INamableAndOrderable
    {
        public Section() { this.classInitializer(); }

        public Section(string _name)  
        {
            this.classInitializer();
            this.name = _name;
            this.order = 0;
        }

        private void classInitializer()
        {
            this.Sections = new List<Section>();
            this.Notes = new List<Note>();
            this.FatherId = null;
            this.name = "";
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string name { get; set; }

        public int order { get; set; }

        [OneToMany(inverseProperty: "Father", CascadeOperations=CascadeOperation.All)]
        public List<Section> Sections { get; set; }

        [ForeignKey(typeof(Section))]
        public int? FatherId { get; set; }

        [OneToMany(inverseProperty: "Father", CascadeOperations = CascadeOperation.All)]
        public List<Note> Notes { get; set; }

        public bool isFinal { get; set; } = true;
    }
}

