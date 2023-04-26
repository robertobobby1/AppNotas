using System;
using AppNotas.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using Android.Media;
using Android.Content.Res;
using static Android.Provider.ContactsContract.CommonDataKinds;
using SQLiteNetExtensions.Extensions;

namespace AppNotas.ViewModels
{
    [QueryProperty(nameof(SongId), nameof(SongId))]
    public class PlayerViewModel : BaseViewModel
	{
        ObservableCollection<Music> musicList;
        public ObservableCollection<Music> MusicList { set => SetProperty(ref musicList, value); get => musicList; }
        private string songId;
        public string SongId { get { return songId; } set { songId = value; init(value); } }
        private Music selectedMusic;
        public Music SelectedMusic { set => SetProperty(ref selectedMusic, value); get => selectedMusic; }
        private TimeSpan duration;
        public TimeSpan Duration { set => SetProperty(ref duration, value); get => duration; }
        private TimeSpan position;
        public TimeSpan Position { set => SetProperty(ref position, value); get => position; }
        double maximum = 100f;
        public double Maximum { set => SetProperty(ref maximum, value); get => maximum; } 
        private bool isPlaying;
        public bool IsPlaying { set => SetProperty(ref isPlaying, value); get => isPlaying; }

        public string PlayIcon { get => isPlaying ? "pause.png" : "play.png"; }
        MediaPlayer player;

        public ICommand PlayCommand => new Command(Play);
        public ICommand ChangeCommand => new Command(ChangeMusic);
        public ICommand BackCommand => new Command(() => Application.Current.MainPage.Navigation.PopAsync());


        public PlayerViewModel()
        {
            MusicList = new ObservableCollection<Music>{
                new Music { Id = 0, title = "Beach Walk", artist = "Unicorn Heads", url = "https://devcrux.com/wp-content/uploads/Beach_Walk.mp3", coverImage = "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcRU6FVly4jMTD3AKB_sHxqPofJVQwqqUj5peEvgA1H4XegM3uJ7&usqp=CAU", isRecent = true },
                new Music { Id = 1, title = "I'll Follow You", artist = "Density & Time", url = "https://devcrux.com/wp-content/uploads/I_ll_Follow_You.mp3", coverImage = "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcRm-su97lHFGZrbR6BkgL32qbzZBj2f3gKGrFR0Pn66ih01SyGj&usqp=CAU" },
                new Music { Id = 2, title = "Ancient", artist = "Density & Time", url = "https://devcrux.com/wp-content/uploads/Ancient.mp3" },
                new Music { Id = 3,  title = "News Room News", artist = "Spence", url = "https://devcrux.com/wp-content/uploads/Cats_Searching_for_the_Truth.mp3" },
                new Music { Id = 4, title = "Bro Time", artist = "Nat Keefe & BeatMowe", url = "https://devcrux.com/wp-content/uploads/Bro_Time.mp3" },
                new Music { Id = 5, title = "Cats Searching for the Truth", artist = "Nat Keefe & Hot Buttered Rum", url = "https://devcrux.com/wp-content/uploads/Cats_Searching_for_the_Truth.mp3" }
            };

            Database.DeleteAll<Music>();
            foreach (Music music in MusicList)
                Database.Insert(music);

            player = new MediaPlayer(); 
        }

        private void init(string songId)
        {
            if (!Int32.TryParse(songId, out int j))
                return;

            SelectedMusic = Database.GetWithChildren<Music>(j);
            if (SelectedMusic == null)
                return;

            PlayMusic(selectedMusic);
            isPlaying = true;
        }

        private void Play()
        {
            if (isPlaying)
            {
                player.Pause();
                IsPlaying = false; ;
            }
            else
            {
                player.Start();
                IsPlaying = true; ;
            }
        }

        private void ChangeMusic(object obj)
        {
            if ((string)obj == "P")
                PreviousMusic();
            else if ((string)obj == "N")
                NextMusic();
        }

        private void PlayMusic(Music music)
        {
            
            IsPlaying = true;

            Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
            {

                return true;
            });
        }

        private void NextMusic()
        {
            var currentIndex = musicList.IndexOf(selectedMusic);

            if (currentIndex < musicList.Count - 1)
            {
                SelectedMusic = musicList[currentIndex + 1];
                PlayMusic(selectedMusic);
            }
        }

        private void PreviousMusic()
        {
            var currentIndex = musicList.IndexOf(selectedMusic);

            if (currentIndex > 0)
            {
                SelectedMusic = musicList[currentIndex - 1];
                PlayMusic(selectedMusic);
            }
        }
    }
} 