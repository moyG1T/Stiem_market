using Stiem_market.Data;
using Stiem_market.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Stiem_market.Pages.Store
{
    /// <summary>
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
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
            if (authViewModel.CartCost > 0)
            {
                if (authViewModel.PayCart() == false)
                {
                    MakeABuyPopup.IsOpen = true;
                }
            }
        }

        private void ShowHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PurchaseHistoryPage(true, authViewModel));
        }
    }
}
