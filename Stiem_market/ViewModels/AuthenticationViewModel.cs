﻿using Stiem_market.Data;
using Stiem_market.PartialClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Stiem_market.ViewModels
{
    public class AuthenticationViewModel : INotifyPropertyChanged
    {
        public AuthenticationViewModel()
        {
            if (Properties.Settings.Default.LoggedUserID == 0)
                LoggedUser = new Users();
            else
                LoggedUser = App.db.Users.Where(x => x.ID == Properties.Settings.Default.LoggedUserID).FirstOrDefault();

            UserCollection = App.db.Users.ToList();

            UpdateUserGames();
        }

        private List<TagInfo> favoriteTags;
        public List<TagInfo> FavoriteTags
        {
            get => favoriteTags;
            set
            {
                favoriteTags = value;
                OnPropertyChanged(nameof(FavoriteTags));
            }
        }

        private Users loggedUser;
        public Users LoggedUser
        {
            get
            {
                if (loggedUser.ID == 0)
                    loggedUser.Nickname = "Вход";
                return loggedUser;
            }
            set
            {
                if (value.ID != 0)
                {
                    SaveLoggedUser(value.ID);
                    IsUserLoggedIn = true;
                }
                else
                {
                    Properties.Settings.Default.Reset();
                    IsUserLoggedIn = false;
                }

                loggedUser = value;
                OnPropertyChanged("LoggedUser");
            }
        }

        public bool IsUserLoggedIn
        {
            get => Properties.Settings.Default.IsUserLoggedIn;
            set
            {
                Properties.Settings.Default.IsUserLoggedIn = value;
                Properties.Settings.Default.Save();
                OnPropertyChanged("IsUserLoggedIn");
            }
        }

        private bool hasFriends;
        public bool HasFriends
        {
            get => hasFriends;
            set
            {
                hasFriends = value;
                OnPropertyChanged(nameof(HasFriends));
            }
        }

        private bool hasGames;
        public bool HasGames
        {
            get => hasGames;
            set
            {
                hasGames = value;
                OnPropertyChanged(nameof(HasGames));
            }
        }

        private Users selectedUser;
        public Users SelectedUser
        {
            get
            {
                return selectedUser;
            }
            set
            {
                selectedUser = value;
                OnPropertyChanged("User");

                FriendsCollection = ConcatFriendList();
                LibraryCollection = App.db.GameInCarts
                    .Where(g => App.db.Carts.Any(c => c.ID == g.Cart_id && c.Owner_id == value.ID && (c.RelationType == 3 || c.RelationType == 5))).ToList();
            }
        }

        private int cartCounter;
        public int CartCounter
        {
            get => cartCounter;
            private set
            {
                if (value <= 0)
                    cartCounter = 0;

                IsCartEmpty = value != 0;

                cartCounter = value;
                OnPropertyChanged(nameof(CartCounter));
            }
        }

        private int cartCost;
        public int CartCost
        {
            get => cartCost;
            private set
            {
                if (value <= 0)
                    cartCost = 0;

                cartCost = value;
                OnPropertyChanged();
            }
        }

        private bool isCartEmpty;
        public bool IsCartEmpty
        {
            get => isCartEmpty;
            private set
            {
                isCartEmpty = value;
                OnPropertyChanged(nameof(IsCartEmpty));
            }
        }

        private IEnumerable<GameInCarts> cartCollection;
        public IEnumerable<GameInCarts> CartCollection
        {
            get => cartCollection;
            set
            {
                value.ToList().ForEach(game => game.InLibrary = LibraryCollection.Any(l => l.Game_id == game.Game_id));

                HasDuplicates = !(value.Any(c => c.InLibrary == true) || value.GroupBy(c => c.Game_id).Any(c => c.Count() > 1));

                cartCollection = value;
                OnPropertyChanged(nameof(CartCollection));
            }
        }

        private bool hasDuplicates;
        public bool HasDuplicates
        {
            get => hasDuplicates;
            private set
            {
                hasDuplicates = value;
                OnPropertyChanged(nameof(HasDuplicates));
            }
        }

        public void UpdateUserGames()
        {
            List<GameInCarts> gameInCarts = App.db.GameInCarts.
                Where(g => App.db.Carts.Any(c => c.ID == g.Cart_id && c.Purchaser_id == LoggedUser.ID && c.RelationType == 2)).ToList();

            CartCounter = gameInCarts.Count();
            CartCost = gameInCarts.Sum(s => s.Games.Cost ?? 0);

            LibraryCollection = App.db.GameInCarts.
                Where(g => App.db.Carts.Any(c => c.ID == g.Cart_id && c.Owner_id == LoggedUser.ID && (c.RelationType == 3 || c.RelationType == 5))).ToList();

            CartCollection = gameInCarts;

            HistoryCollection = App.db.Carts.Where(h => (h.Purchaser_id == LoggedUser.ID || h.Owner_id == LoggedUser.ID) && (h.RelationType == 3 || h.RelationType == 5)).OrderByDescending(x => x.AddDate).ToList();

            FavoriteTags = App.db.Users.Where(u => u.ID == LoggedUser.ID)
                                       .SelectMany(u => u.Carts.Where(с => с.RelationType == 3))
                                       .SelectMany(c => c.GameInCarts)
                                       .SelectMany(gic => gic.Games.GameTags)
                                       .GroupBy(gt => new { gt.Tags.ID, gt.Tags.Title })
                                       .Select(group => new TagInfo { ID = group.Key.ID, Title = group.Key.Title, Count = group.Count() })
                                       .OrderByDescending(x => x.Count)
                                       .ToList();
        }

        public void PayCart(Users user)
        {
            // Покупка себе
            if (user.ID == LoggedUser.ID)
            {
                Carts cart = App.db.Carts.Where(c => c.Purchaser_id == LoggedUser.ID && c.RelationType == 2).FirstOrDefault();
                cart.RelationType = 3;
                cart.Owner_id = user.ID;
                cart.AddDate = DateTime.Now;
                // Консервация цен продуктов при покупке
                App.db.GameInCarts.Where(x => x.Carts.Purchaser_id == LoggedUser.ID && x.Carts.RelationType == 2)
                                  .ToList().ForEach(gic => gic.PurchasedCost = gic.Games.Cost);
            }
            // Покупка в подарок
            else
            {
                Carts cart = App.db.Carts.Where(c => c.Purchaser_id == LoggedUser.ID && c.RelationType == 4).FirstOrDefault();
                cart.RelationType = 5;
                cart.Owner_id = user.ID;
                cart.AddDate = DateTime.Now;
                // Консервация цен продуктов при покупке
                App.db.GameInCarts.Where(x => x.Carts.Purchaser_id == LoggedUser.ID && x.Carts.RelationType == 4)
                                  .ToList().ForEach(gic => gic.PurchasedCost = gic.Games.Cost);
            }

            LoggedUser.Balance -= CartCost;

            App.db.SaveChanges();

            UpdateUserGames();
        }

        private IEnumerable<GameInCarts> libraryCollection;
        public IEnumerable<GameInCarts> LibraryCollection
        {
            get => libraryCollection;
            set
            {
                HasGames = value.Count() == 0;
                libraryCollection = value;
                OnPropertyChanged(nameof(LibraryCollection));
            }
        }

        private IEnumerable<Carts> historyCollection;
        public IEnumerable<Carts> HistoryCollection
        {
            get => historyCollection;
            set
            {
                historyCollection = value;
                OnPropertyChanged(nameof(HistoryCollection));
            }
        }

        private IEnumerable<Users> searchCollection;
        public IEnumerable<Users> SearchCollection
        {
            get => searchCollection ?? UserCollection;
            set
            {
                searchCollection = value;
                OnPropertyChanged("SearchCollection");
            }
        }

        private string searchUsersText;
        public string SearchUsersText
        {
            get => searchUsersText;
            set
            {
                if (value == "")
                    SearchCollection = App.db.Users.ToList();
                else
                    SearchCollection = App.db.Users.Where(x => x.Nickname.ToLower().Contains(value.ToLower())).ToList();

                searchUsersText = value;
                OnPropertyChanged(nameof(SearchUsersText));
            }
        }

        private string searchFriendsText;
        public string SearchFriendsText
        {
            get => searchFriendsText;
            set
            {
                searchFriendsText = value;
                OnPropertyChanged(nameof(SearchFriendsText));

                RelationFilter();
            }
        }

        private int selectedFilter = 0;
        public int SelectedFilter
        {
            get => selectedFilter;
            set
            {
                selectedFilter = value;
                OnPropertyChanged(nameof(SelectedFilter));

                RelationFilter();
            }
        }

        public void RelationFilter()
        {
            var query = App.db.Users.Where(u => App.db.FriendUsers.
                Any(fu =>
                    fu.User_id == SelectedUser.ID && fu.Friend_id == u.ID && fu.RelationType == 2 &&
                    (
                        (fu.Sender_id != LoggedUser.ID && SelectedFilter == 1) ||
                        (fu.Sender_id == LoggedUser.ID && SelectedFilter == 2)
                    )
                    ||
                    fu.Friend_id == SelectedUser.ID && fu.User_id == u.ID && fu.RelationType == 2 &&
                    (
                        (fu.Sender_id != LoggedUser.ID && SelectedFilter == 1) ||
                        (fu.Sender_id == LoggedUser.ID && SelectedFilter == 2)
                    )
                )).ToList();

            if (string.IsNullOrEmpty(SearchFriendsText))
            {
                SearchFriendsCollection = SelectedFilter == 0 ? FriendsCollection : query;
            }
            else
            {
                SearchFriendsCollection = SelectedFilter == 0 ?
                    FriendsCollection.Where(x => x.Nickname.ToLower().Contains(SearchFriendsText.ToLower()))
                    :
                    query.Where(x => x.Nickname.ToLower().Contains(SearchFriendsText.ToLower()));
            }
        }

        private IEnumerable<Users> searchFriendsCollection;
        public IEnumerable<Users> SearchFriendsCollection
        {
            get => searchFriendsCollection ?? FriendsCollection;
            set
            {
                searchFriendsCollection = value;
                OnPropertyChanged(nameof(SearchFriendsCollection));
            }
        }

        private IEnumerable<Users> userCollection;
        public IEnumerable<Users> UserCollection
        {
            get => userCollection;
            set
            {
                userCollection = value;
                OnPropertyChanged(nameof(UserCollection));
            }
        }

        private IEnumerable<Users> friendsCollection;
        public IEnumerable<Users> FriendsCollection
        {
            get => friendsCollection;
            set
            {
                friendsCollection = value;
                // обновляем список поисковика друзей
                SearchFriendsText = "";
                SearchFriendsCollection = value;
                OnPropertyChanged(nameof(FriendsCollection));
            }
        }

        public IEnumerable<Users> ConcatFriendList()
        {
            IEnumerable<Users> users = App.db.Users.Where(u => App.db.FriendUsers.
                                        Any(fu => fu.User_id == SelectedUser.ID && fu.Friend_id == u.ID && fu.RelationType == 3 ||
                                            fu.Friend_id == SelectedUser.ID && fu.User_id == u.ID && fu.RelationType == 3)).ToList();


            HasFriends = !(users.Count() > 0);

            return users;
        }

        public void SaveLoggedUser(int id)
        {
            Properties.Settings.Default.LoggedUserID = id;
            Properties.Settings.Default.Save();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
