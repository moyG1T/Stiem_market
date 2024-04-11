using Stiem.ViewModels;
using Stiem_market.Data;
using Stiem_market.Pages.Store;
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

            authViewModel = _authViewModel;

            if (authViewModel.LoggedUser.ID == authViewModel.SelectedUser.ID)
            {
                EditProfile.Visibility = Visibility.Visible;
            }
            else
            {
                EditProfile.Visibility = Visibility.Collapsed;
                RefreshFriendButtons();
            }

            DataContext = authViewModel;
        }

        private void RefreshFriendButtons()
        {
            var relation = App.db.FriendUsers.Where(x => x.User_id == authViewModel.LoggedUser.ID && x.Friend_id == authViewModel.SelectedUser.ID
                                                      || x.Friend_id == authViewModel.LoggedUser.ID && x.User_id == authViewModel.SelectedUser.ID).FirstOrDefault();

            InviteUser.Visibility = Visibility.Collapsed;
            RemoveFriend.Visibility = Visibility.Collapsed;
            RemoveRequest.Visibility = Visibility.Collapsed;
            AcceptRequest.Visibility = Visibility.Collapsed;

            if (relation == null)
            {
                InviteUser.Visibility = Visibility.Visible;
                return;
            }

            if (relation.RelationType == 1)
            {
                InviteUser.Visibility = Visibility.Visible;
            }
            else if (relation.RelationType == 3)
            {
                RemoveFriend.Visibility = Visibility.Visible;
            }
            else if (relation.RelationType == 2 && relation.Sender_id == authViewModel.LoggedUser.ID)
            {
                RemoveRequest.Visibility = Visibility.Visible;
            }
            else if (relation.RelationType == 2 && relation.Sender_id == authViewModel.SelectedUser.ID)
            {
                AcceptRequest.Visibility = Visibility.Visible;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (canNavigateBack)
                NavigationService.GoBack();
            authViewModel.SelectedUser = authViewModel.LoggedUser;
        }

        private void EditProfile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditUserProfile(authViewModel));
        }

        private void InviteUser_Click(object sender, RoutedEventArgs e)
        {
            // проверка, что записи между пользователями нет
            if (App.db.FriendUsers.Where(x => x.User_id == authViewModel.LoggedUser.ID && x.Friend_id == authViewModel.SelectedUser.ID
                                           || x.Friend_id == authViewModel.LoggedUser.ID && x.User_id == authViewModel.SelectedUser.ID)
                                              .FirstOrDefault() == null)
            {
                // заявка отправлена
                App.db.FriendUsers.Add(new FriendUsers()
                {
                    User_id = authViewModel.LoggedUser.ID,
                    Friend_id = authViewModel.SelectedUser.ID,
                    RelationType = 2,
                    Sender_id = authViewModel.LoggedUser.ID
                });
            }
            else
            {
                var user = App.db.FriendUsers.Where(x =>
                                            x.User_id == authViewModel.LoggedUser.ID
                                         && x.Friend_id == authViewModel.SelectedUser.ID
                                         || x.Friend_id == authViewModel.LoggedUser.ID
                                         && x.User_id == authViewModel.SelectedUser.ID).FirstOrDefault();
                user.RelationType = 2;
                user.Sender_id = authViewModel.LoggedUser.ID;
            }

            App.db.SaveChanges();
            RefreshFriendButtons();
        }

        private void RemoveFriend_Click(object sender, RoutedEventArgs e)
        {
            var user = App.db.FriendUsers.Where(x =>
                                            x.User_id == authViewModel.LoggedUser.ID
                                         && x.Friend_id == authViewModel.SelectedUser.ID
                                         || x.Friend_id == authViewModel.LoggedUser.ID
                                         && x.User_id == authViewModel.SelectedUser.ID).FirstOrDefault();
            user.RelationType = 1;
            user.AddDate = null;
            user.Sender_id = null;

            App.db.SaveChanges();
            RefreshFriendButtons();
        }

        private void AcceptRequest_Click(object sender, RoutedEventArgs e)
        {
            var user = App.db.FriendUsers.Where(x =>
                                             x.User_id == authViewModel.LoggedUser.ID
                                          && x.Friend_id == authViewModel.SelectedUser.ID
                                          || x.Friend_id == authViewModel.LoggedUser.ID
                                          && x.User_id == authViewModel.SelectedUser.ID).FirstOrDefault();
            user.RelationType = 3;
            user.AddDate = DateTime.Now;

            App.db.SaveChanges();
            RefreshFriendButtons();
        }

        private void RemoveRequest_Click(object sender, RoutedEventArgs e)
        {
            var user = App.db.FriendUsers.Where(x =>
                                            x.User_id == authViewModel.LoggedUser.ID
                                         && x.Friend_id == authViewModel.SelectedUser.ID
                                         || x.Friend_id == authViewModel.LoggedUser.ID
                                         && x.User_id == authViewModel.SelectedUser.ID).FirstOrDefault();
            user.RelationType = 1;
            user.AddDate = null;
            user.Sender_id = null;

            App.db.SaveChanges();
            RefreshFriendButtons();
        }

        private void FriendList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            authViewModel.SelectedUser = FriendList.SelectedItem as Users;
            NavigationService.Navigate(new UserProfile(true, authViewModel));
        }

        private void GameList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            gameViewModel = new GameViewModel(authViewModel.FavoriteTags);
            gameViewModel.SelectedGame = (GameList.SelectedItem as GameInCarts).Games;
            NavigationService.Navigate(new GamePage(true, authViewModel, gameViewModel));
        }
    }
}
