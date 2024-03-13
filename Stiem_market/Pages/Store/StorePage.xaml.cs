using Stiem.ViewModels;
using Stiem_market.Data;
using Stiem_market.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Stiem_market.Pages.Store
{
    /// <summary>
    /// Логика взаимодействия для StorePage.xaml
    /// </summary>
    public partial class StorePage : Page
    {
        private GameViewModel gameViewModel;
        private AuthenticationViewModel authViewModel;

        public StorePage(bool _canNavigateBack, AuthenticationViewModel _authViewModel, GameViewModel _gameViewModel)
        {
            InitializeComponent();

            gameViewModel = _gameViewModel;
            authViewModel = _authViewModel;
            DataContext = gameViewModel;
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GameList.SelectedItem == null)
                return;
            gameViewModel.SelectedGame = GameList.SelectedItem as Games;
            NavigationService.Navigate(new GamePage(true, authViewModel, gameViewModel));
            GameList.SelectedItem = null;
        }
    }
}
