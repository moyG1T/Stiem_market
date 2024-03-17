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
    /// Логика взаимодействия для GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        private bool canNavigateBack;
        private GameViewModel gameViewModel;
        private AuthenticationViewModel authViewModel;

        public GamePage(bool _canNavigateBack, AuthenticationViewModel _authViewModel, GameViewModel _gameViewModel)
        {
            InitializeComponent();

            canNavigateBack = _canNavigateBack;
            gameViewModel = _gameViewModel;
            authViewModel = _authViewModel;

            DataContext = gameViewModel;

            if (gameViewModel.SelectedGame != null)
            {
                if (authViewModel.LibraryCollection.Any(x => x.Game_id == gameViewModel.SelectedGame.ID))
                {
                    InfoText.Text = "Товар в библиотеке";
                    AddToCart.IsEnabled = false;
                }
                else if (authViewModel.CartCollection.Any(x => x.Game_id == gameViewModel.SelectedGame.ID))
                {
                    InfoText.Text = "Товар в корзине";
                    AddToCart.IsEnabled = false;
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (canNavigateBack)
            {
                NavigationService.GoBack();
                gameViewModel.SelectedGame = new Games();
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            Carts cart = App.db.Carts.Where(c => c.Purchaser_id == authViewModel.LoggedUser.ID && c.RelationType == 2).FirstOrDefault();
            if (cart == null)
            {
                App.db.Carts.Add(new Carts()
                {
                    Purchaser_id = authViewModel.LoggedUser.ID,
                    RelationType = 2,
                });
                App.db.SaveChanges();
                cart = App.db.Carts.Where(c => c.Purchaser_id == authViewModel.LoggedUser.ID && c.RelationType == 2).FirstOrDefault();
            }

            App.db.GameInCarts.Add(new GameInCarts()
            {
                Game_id = gameViewModel.SelectedGame.ID,
                Cart_id = cart.ID,
            });
            App.db.SaveChanges();
            authViewModel.UpdateUserGames();

            CartPopup.IsOpen = true;
            InfoText.Text = "Товар в корзине";
            AddToCart.IsEnabled = false;
        }
    }
}