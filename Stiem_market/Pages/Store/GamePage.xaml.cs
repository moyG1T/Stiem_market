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

            // проверка на существование записи пользователя и игры
            if (gameViewModel.SelectedGame != null
                && gameViewModel.SelectedGame.UserGames.Where(x => x.User_id == authViewModel.LoggedUser.ID).FirstOrDefault() != null)
            {
                if (gameViewModel.SelectedGame.UserGames.Where(x => x.User_id == authViewModel.LoggedUser.ID).FirstOrDefault().RelationType == 4)
                {
                    InfoText.Text = "Товар в библиотеке";
                    AddToCart.IsEnabled = false;
                }
                else if (gameViewModel.SelectedGame.UserGames.Where(x => x.User_id == authViewModel.LoggedUser.ID).FirstOrDefault().RelationType == 2)
                {
                    InfoText.Text = "Товар в корзине";
                    AddToCart.IsEnabled = false;
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (canNavigateBack)
                NavigationService.GoBack();
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            // если пользователь никак не взаимодействовал с игрой
            if (App.db.UserGames.Where(x => x.Game_id == gameViewModel.SelectedGame.ID && x.User_id == authViewModel.LoggedUser.ID).FirstOrDefault() == null)
            {
                App.db.UserGames.Add(new UserGames()
                {
                    User_id = authViewModel.LoggedUser.ID,
                    Game_id = gameViewModel.SelectedGame.ID,
                    IsWished = false,
                    RelationType = 2,
                });
                App.db.SaveChanges();
                authViewModel.UpdateUserGames();
            }
            // изменение существующей записи
            else
            {
                if (authViewModel.LoggedUser.UserGames.
                        Where(x => x.Game_id == gameViewModel.SelectedGame.ID && x.User_id == authViewModel.LoggedUser.ID).FirstOrDefault().
                        RelationType != 2)
                {
                    authViewModel.LoggedUser.UserGames.
                        Where(x => x.Game_id == gameViewModel.SelectedGame.ID && x.User_id == authViewModel.LoggedUser.ID).FirstOrDefault().
                        RelationType = 2;

                    App.db.SaveChanges();

                    authViewModel.UpdateUserGames();
                }
            }

            CartPopup.IsOpen = true;
            InfoText.Text = "Товар в корзине";
            AddToCart.IsEnabled = false;
        }
    }
}