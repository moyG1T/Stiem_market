using Stiem_market.Data;
using Stiem_market.Pages.Authentication;
using Stiem_market.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Stiem_market.Pages.FriendTab
{
    /// <summary>
    /// Логика взаимодействия для SearchUsers.xaml
    /// </summary>
    public partial class SearchUsers : Page
    {
        private AuthenticationViewModel authViewModel;

        public SearchUsers(bool _canNavigateBack, AuthenticationViewModel _authViewModel)
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
            if (string.IsNullOrEmpty(authViewModel.SearchableText))
                CancelButton.IsEnabled = false;
            else
                CancelButton.IsEnabled = true;
        }
    }
}
