using Stiem_market.Data;
using Stiem_market.ViewModels;
using Stiem_market.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Stiem_market.Pages.Store
{
    /// <summary>
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        private FriendListWindow friendListWindow;

        private AuthenticationViewModel authViewModel;
        public CartPage(AuthenticationViewModel _authViewModel)
        {
            InitializeComponent();
            authViewModel = _authViewModel;

            DataContext = authViewModel;
            CartListBox.DataContext = authViewModel;
        }


        private void RemoveFromCartButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            App.db.GameInCarts.Remove((GameInCarts)button.DataContext);
            App.db.SaveChanges();

            authViewModel.UpdateUserGames();
        }

        private void MakeABuyButton_Click(object sender, RoutedEventArgs e)
        {
            if (authViewModel.CartCost > authViewModel.LoggedUser.Balance)
            {
                BalanceUnderCost.IsOpen = true;
                return;
            }

            if (authViewModel.CartCost > 0)
                authViewModel.PayCart(authViewModel.LoggedUser);
        }

        private void ShowHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PurchaseHistoryPage(true, authViewModel));
        }

        private void MakeABuyForFriendButton_Click(object sender, RoutedEventArgs e)
        {
            if (authViewModel.CartCost > authViewModel.LoggedUser.Balance)
            {
                BalanceUnderCost.IsOpen = true;
                return;
            }

            if (authViewModel.CartCost > 0)
            {
                if (friendListWindow == null || !friendListWindow.IsVisible)
                {
                    friendListWindow = new FriendListWindow(authViewModel);

                    friendListWindow.Closed += (s, args) =>
                    {
                        MakeABuyButton.IsEnabled = true;
                        MakeABuyForFriendButton.IsEnabled = true;
                    };

                    friendListWindow.Show();
                    MakeABuyButton.IsEnabled = false;
                    MakeABuyForFriendButton.IsEnabled = false;
                }
            }
        }
    }
}
