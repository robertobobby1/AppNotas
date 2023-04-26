using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppNotas.Models;
using SQLiteNetExtensions.Extensions;

namespace AppNotas.Dependencies
{
	public class MemoryManager
	{
        public MemoryManager()
        {
            FatherSection = null;
            Navigation = new Stack<Section>();
        }

        private List<Section> sections;
        public List<Section> Sections {
			private set { sections = value; }
            get { return sections; }
		}

		public Stack<Section> Navigation { get; set; }
		public Section FatherSection { get; set; }

		/*
		 * If sectionId is null will navigate backwards
		 */
		public void navigateToSection(int? sectionId)
		{
			if (sectionId == null)
			{
				Navigation.Pop();
				if (Navigation.Count == 0)
					FatherSection = null;
				else 
					FatherSection = Navigation.Peek();
                return;
			}

			var section = new Section();
			if (FatherSection == null)
				section = sections.Find(x => x.Id == sectionId);
			else if (FatherSection.Id == sectionId)
				return;
			else
				section = FatherSection.Sections.Find(x => x.Id == sectionId);

            Navigation.Push(section);
			FatherSection = section;
		}

		public void navigateHome()
		{
			while (FatherSection != null)
				navigateToSection(null);
		}

		/*
		 * Set to null so that the memory is freed and then get again to load.
		 */
        public async void reloadSectionsAsync()
        {
			await Task.Run(loadSectionsSync);
        }

		public void loadSectionsSync()
		{
            sections = new SQLiteDefaultConnection()
				.GetAllWithChildren<Section>(i => i.FatherId == null, true); 
        }
    }
}

