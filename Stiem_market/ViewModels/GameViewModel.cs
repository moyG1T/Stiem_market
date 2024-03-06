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
        public GameViewModel()
        {
            Games = App.db.Games.ToList();
        }

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
