using Stiem_market.Data;
using Stiem_market.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Stiem_market.Pages.Library
{
    /// <summary>
    /// Логика взаимодействия для LibraryPage.xaml
    /// </summary>
    public partial class LibraryPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private GameInCarts selectedGame;
        public GameInCarts SelectedGame
        {
            get => selectedGame;
            set
            {
                selectedGame = value;

                if (selectedGame != null)
                {
                    IsGameSelected = true;
                }
                else
                {
                    IsGameSelected = false;
                }

                OnPropertyChanged(nameof(SelectedGame));
            }
        }

        private bool isGameSelected = false;
        public bool IsGameSelected
        {
            get => isGameSelected;
            set
            {
                isGameSelected = value;
                OnPropertyChanged(nameof(IsGameSelected));
            }
        }

        public ObservableCollection<GameInCarts> LibraryCollection { get; set; }

        public LibraryPage(bool _canNavigateBack, IEnumerable<GameInCarts> gameList)
        {
            InitializeComponent();

            LibraryCollection = new ObservableCollection<GameInCarts>(gameList);
            DataContext = this;
        }

        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
