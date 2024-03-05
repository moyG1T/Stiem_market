using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Stiem_market;
using Stiem_market.Data;

namespace Stiem.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
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

        public ObservableCollection<Games> Games { get; set; }

        public GameViewModel()
        {
            Games = new ObservableCollection<Games>(App.db.Games);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
