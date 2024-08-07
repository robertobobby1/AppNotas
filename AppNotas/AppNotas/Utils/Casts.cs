using System;
using AppNotas.Models;

namespace AppNotas.Utils
{
	public class Casts
	{
		public Casts()
		{ 
        }

        public static Note getNote(INamableAndOrderable toCast)
        {
            Note aux = (Note)Convert.ChangeType(toCast, typeof(Note));
            return aux;
        }

        public static Section getSection(INamableAndOrderable toCast)
        {
            Section aux = (Section)Convert.ChangeType(toCast, typeof(Section));
            return aux;
        }

    }
}

