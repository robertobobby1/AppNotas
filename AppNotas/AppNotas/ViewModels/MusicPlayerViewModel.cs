using AppNotas.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using AppNotas.Views;

namespace AppNotas.ViewModels
{
	public class MusicPlayerViewModel : BaseViewModel
	{
        public ICommand SelectionCommand => new Command(PlayMusic);

        public MusicPlayerViewModel()
		{
            MusicList = new ObservableCollection<Music>{
                new Music { Id = 0, title = "Beach Walk", artist = "Unicorn Heads", url = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-17.mp3", coverImage = "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcRU6FVly4jMTD3AKB_sHxqPofJVQwqqUj5peEvgA1H4XegM3uJ7&usqp=CAU", isRecent = true },
                new Music { Id = 1, title = "I'll Follow You", artist = "Density & Time", url = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-16.mp3", coverImage = "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcRm-su97lHFGZrbR6BkgL32qbzZBj2f3gKGrFR0Pn66ih01SyGj&usqp=CAU" },
                new Music { Id = 2, title = "Ancient", artist = "Density & Time", url = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-15.mp3" },
                new Music { Id = 3,  title = "News Room News", artist = "Spence", url = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-14.mp3" },
                new Music { Id = 4, title = "Bro Time", artist = "Nat Keefe & BeatMowe", url = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-13.mp3" },
                new Music { Id = 5, title = "Cats Searching for the Truth", artist = "Nat Keefe & Hot Buttered Rum", url = "https://www.soundhelix.com/examples/mp3/SoundHelix-Song-12.mp3" }
            };

            recentMusic = musicList.Where(x => x.isRecent == true).FirstOrDefault();
        }

        ObservableCollection<Music> musicList;
        public ObservableCollection<Music> MusicList
        {
            get { return musicList; }
            set
            {
                musicList = value;
                OnPropertyChanged();
            }
        }

        private Music recentMusic;
        public Music RecentMusic
        {
            get { return recentMusic; }
            set
            {
                recentMusic = value;
                OnPropertyChanged();
            }
        }

        private Music selectedMusic;
        public Music SelectedMusic
        {
            get { return selectedMusic; }
            set
            {
                selectedMusic = value;
                OnPropertyChanged();
            }
        }

        private void PlayMusic()
        {
            if (selectedMusic != null)
            {
                Shell.Current.GoToAsync($"{nameof(PlayerPage)}?{nameof(PlayerViewModel.SongId)}={selectedMusic.Id}");
            }
        }
    }
}