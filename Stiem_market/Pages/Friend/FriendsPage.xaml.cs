using Stiem_market.Data;
using Stiem_market.Pages.Authentication;
using Stiem_market.Pages.FriendTab;
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

namespace Stiem_market.Pages.Friend
{
    /// <summary>
    /// Логика взаимодействия для FriendsPage.xaml
    /// </summary>
    public partial class FriendsPage : Page
    {
        private AuthenticationViewModel authViewModel;

        public FriendsPage(bool _canNavigateBack, AuthenticationViewModel _authViewModel)
        {
            InitializeComponent();
            SearchBar.Focus();

            authViewModel = _authViewModel;
            DataContext = authViewModel;

            CancelButton.IsEnabled = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SearchBar.Text = "";
        }

        private void UserList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            authViewModel.SelectedUser = UserList.SelectedItem as Users;
            NavigationService.Navigate(new UserProfile(true, authViewModel));
        }

        private void SearchBar_Loaded(object sender, RoutedEventArgs e)
        {
            SearchBar.CaretIndex = SearchBar.Text.Length;
            SearchBar.ScrollToEnd();
            SearchBar.Focus();
            CancelButtonAbility();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            CancelButtonAbility();
        }

        private void CancelButtonAbility()
        {
            if (string.IsNullOrEmpty(authViewModel.SearchUsersText))
                CancelButton.IsEnabled = false;
            else
                CancelButton.IsEnabled = true;
        }

        private void FindNewFriendsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SearchUsers(true, authViewModel));
        }

        private void FriendFilter_Loaded(object sender, RoutedEventArgs e)
        {
            authViewModel.RelationFilter();
        }
    }
}
