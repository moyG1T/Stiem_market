using Stiem.ViewModels;
using Stiem_market.ViewModels;
using System.Windows;
using Stiem_market.Pages.Authentication;
using Stiem_market.Pages.Library;
using Stiem_market.Pages.Store;
using Stiem_market.Data;
using Stiem_market.Pages.FriendTab;
using Stiem_market.Pages.Friend;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Stiem_market
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AuthenticationViewModel authViewModel;
        private GameViewModel gameViewModel;

        public MainWindow()
        {
            InitializeComponent();

            authViewModel = new AuthenticationViewModel();
            gameViewModel = new GameViewModel();
            DataContext = authViewModel;
            CartButton.DataContext = authViewModel;
            //Properties.Settings.Default.Reset();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AccountTab.IsChecked = true;
        }

        private void StoreTab_Checked(object sender, RoutedEventArgs e)
        {
            StiemFrame.Navigate(new StorePage(false, authViewModel, gameViewModel));
        }

        private void LibraryTab_Checked(object sender, RoutedEventArgs e)
        {
            StiemFrame.Navigate(new LibraryPage(false, authViewModel));
        }

        private void FriendsTab_Checked(object sender, RoutedEventArgs e)
        {
            StiemFrame.Navigate(new FriendsPage(false, authViewModel));
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            StiemFrame.Navigate(new CartPage(authViewModel));
        }

        private void AccountTab_Checked(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.LoggedUserID == 0)
                StiemFrame.Navigate(new AuthorizationPage(false, authViewModel));
            else
            {
                authViewModel.SelectedUser = authViewModel.LoggedUser;
                StiemFrame.Navigate(new UserProfile(false, authViewModel));
            }
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            authViewModel.LoggedUser = new Users();
            StiemFrame.Navigate(new AuthorizationPage(false, authViewModel));
            AccountTab.IsChecked = true;
        }
    }
}
