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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Stiem_market.Pages.Authentication
{
    /// <summary>
    /// Логика взаимодействия для UserProfile.xaml
    /// </summary>
    public partial class UserProfile : Page
    {
        private AuthenticationViewModel authViewModel;
        private GameViewModel gameViewModel;
        private bool canNavigateBack;

        public UserProfile(bool _canNavigateBack, AuthenticationViewModel _authViewModel)
        {
            InitializeComponent();

            canNavigateBack = _canNavigateBack;
            if (canNavigateBack)
                BackButton.Visibility = Visibility.Visible;
            else
                BackButton.Visibility = Visibility.Hidden;

            gameViewModel = new GameViewModel();
            authViewModel = _authViewModel;

            if (authViewModel.LoggedUser.ID == authViewModel.SelectedUser.ID)
                EditProfile.Visibility = Visibility.Visible;
            else
                EditProfile.Visibility = Visibility.Hidden;

            DataContext = authViewModel;
            GameList.DataContext = gameViewModel;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (canNavigateBack)
                NavigationService.GoBack();
            authViewModel.SelectedUser = new Users();
        }

        private void EditProfile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditUserProfile(authViewModel));
        }
    }
}
