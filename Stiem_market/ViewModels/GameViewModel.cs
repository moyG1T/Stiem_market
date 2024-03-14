using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Stiem_market;
using Stiem_market.Data;

namespace Stiem.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        public GameViewModel(List<int> favTags)
        {
            Games = App.db.Games.ToList();

            if (favTags.Count() > 0)
            {
                int index = favTags.ElementAt(0);
                Top1FavoriteTag = App.db.Tags.Where(f => f.ID == index).FirstOrDefault().Title;
                Top1FavoriteGames =
                    new ObservableCollection<Games>(Games.Where(g => g.GameTags.Any(gt => gt.Game_id == g.ID && gt.Tags.ID == index)));
            }
            if (favTags.Count() > 1)
            {
                int index = favTags.ElementAt(1);
                Top2FavoriteTag = App.db.Tags.Where(f => f.ID == index).FirstOrDefault().Title;
                Top2FavoriteGames =
                    new ObservableCollection<Games>(Games.Where(g => g.GameTags.Any(gt => gt.Game_id == g.ID && gt.Tags.ID == index)));
            }
            if (favTags.Count() > 2)
            {
                int index = favTags.ElementAt(2);
                Top3FavoriteTag = App.db.Tags.Where(f => f.ID == index).FirstOrDefault().Title;
                Top3FavoriteGames =
                    new ObservableCollection<Games>(Games.Where(g => g.GameTags.Any(gt => gt.Game_id == g.ID && gt.Tags.ID == index)));
            }
        }

        public string Top1FavoriteTag { get; set; }
        public ObservableCollection<Games> Top1FavoriteGames { get; set; }

        public string Top2FavoriteTag { get; set; }
        public ObservableCollection<Games> Top2FavoriteGames { get; set; }

        public string Top3FavoriteTag { get; set; }
        public ObservableCollection<Games> Top3FavoriteGames { get; set; }


        private ObservableCollection<Games> _selectedGames;
        public ObservableCollection<Games> SelectedGames
        {
            get
            {
                return _selectedGames;
            }
            set
            {
                _selectedGames = value;
                OnPropertyChanged("SelectedGames");
            }
        }

        private Games selectedGame;
        public Games SelectedGame
        {
            get => selectedGame;
            set
            {
                selectedGame = value;
                OnPropertyChanged("SelectedGame");
            }
        }

        private IEnumerable<Games> games;
        public IEnumerable<Games> Games
        {
            get => games;
            set
            {
                games = value;
                OnPropertyChanged(nameof(Games));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
