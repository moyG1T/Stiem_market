using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Stiem_market;
using Stiem_market.Data;
using Stiem_market.PartialClasses;

namespace Stiem.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        public GameViewModel(List<TagInfo> favTags)
        {
            Games = App.db.Games.ToList();

            if (favTags.Count() > 0)
            {
                FavoriteTag = favTags.ElementAt(0).Title;
                TopFavoriteGames =
                    new ObservableCollection<Games>(Games.Where(g => g.GameTags.Any(gt => gt.Game_id == g.ID && gt.Tags.ID == favTags.ElementAt(0).ID)));
            }
        }

        public string FavoriteTag { get; set; }
        public ObservableCollection<Games> TopFavoriteGames { get; set; }

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
