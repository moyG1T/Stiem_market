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
using System.IO;

namespace Stiem_market
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AuthenticationViewModel authViewModel;

        public MainWindow()
        {
            InitializeComponent();

            authViewModel = new AuthenticationViewModel();
            DataContext = authViewModel;
            CartButton.DataContext = authViewModel;
            //Properties.Settings.Default.Reset();

            //App.db.GameShowcase.Where(x => x.Game_id == 1).FirstOrDefault().Image = File.ReadAllBytes(@"C:\Users\Welcome\source\repos\Stiem_market\Stiem_market\Resources\sussy.png");
            //App.db.SaveChanges();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AccountTab.IsChecked = true;
        }

        private void StoreTab_Checked(object sender, RoutedEventArgs e)
        {
            authViewModel.SelectedUser = authViewModel.LoggedUser;
            StiemFrame.Navigate(new StorePage(false, authViewModel));
        }

        private void LibraryTab_Checked(object sender, RoutedEventArgs e)
        {
            authViewModel.SelectedUser = authViewModel.LoggedUser;
            StiemFrame.Navigate(new LibraryPage(false, authViewModel.LibraryCollection));
        }

        private void FriendsTab_Checked(object sender, RoutedEventArgs e)
        {
            authViewModel.SelectedUser = authViewModel.LoggedUser;
            StiemFrame.Navigate(new FriendsPage(false, authViewModel));
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            authViewModel.SelectedUser = authViewModel.LoggedUser;
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
