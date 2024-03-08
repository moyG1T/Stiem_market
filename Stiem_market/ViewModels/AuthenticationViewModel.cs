using Stiem_market.Data;
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
            get { return Properties.Settings.Default.IsUserLoggedIn; }
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
                cartCollection = value;
                OnPropertyChanged(nameof(CartCollection));
            }
        }

        public void UpdateUserGames()
        {
            List<GameInCarts> gameInCarts = App.db.GameInCarts.
                Where(g => App.db.Carts.Any(c => c.ID == g.Cart_id && c.User_id == LoggedUser.ID && c.RelationType == 2)).ToList();

            CartCounter = gameInCarts.Count();
            IsCartEmpty = CartCounter != 0;

            int sum = 0;
            foreach (var item in gameInCarts)
                sum += item.Games.Cost ?? 0;
            CartCost = sum;

            CartCollection = gameInCarts;

            LibraryCollection = App.db.GameInCarts.
                Where(g => App.db.Carts.Any(c => c.ID == g.Cart_id && c.User_id == LoggedUser.ID && c.RelationType == 3)).ToList();
            HasGames = !(LibraryCollection.Count() > 0);

            HistoryCollection = App.db.Carts.Where(h => h.User_id == LoggedUser.ID && h.RelationType == 3).ToList();
        }

        public bool PayCart()
        {
            if (App.db.Users.Where(x => x.ID == LoggedUser.ID).FirstOrDefault().Balance < CartCost)
            {
                return false;
            }
            else
            {
                App.db.Carts.Where(c => c.User_id == LoggedUser.ID && c.RelationType == 2).FirstOrDefault().RelationType = 3;
                App.db.Carts.Where(c => c.User_id == LoggedUser.ID && c.RelationType == 2).FirstOrDefault().AddDate = DateTime.Now;
                App.db.Carts.Where(c => c.User_id == LoggedUser.ID && c.RelationType == 2).FirstOrDefault().CartCost = -CartCost;

                LoggedUser.Balance -= CartCost;

                App.db.SaveChanges();

                UpdateUserGames();
                return true;
            }
        }

        private IEnumerable<GameInCarts> libraryCollection;
        public IEnumerable<GameInCarts> LibraryCollection
        {
            get => libraryCollection;
            set
            {
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

        private string searchableText;
        public string SearchableText
        {
            get => searchableText;
            set
            {
                if (value == "")
                    SearchCollection = App.db.Users.ToList();
                else
                    SearchCollection = App.db.Users.Where(x => x.Nickname.ToLower().Contains(value.ToLower())).ToList();

                searchableText = value;
                OnPropertyChanged(nameof(SearchableText));
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
                OnPropertyChanged(nameof(FriendsCollection));
            }
        }

        public IEnumerable<Users> ConcatFriendList()
        {
            List<int> IDs = new List<int>();
            List<Users> users = new List<Users>();

            foreach (var item in App.db.FriendUsers.Where(x => x.User_id == SelectedUser.ID && x.RelationType == 3))
                IDs.Add((int)item.Friend_id);

            foreach (var item in App.db.FriendUsers.Where(x => x.Friend_id == SelectedUser.ID && x.RelationType == 3))
                IDs.Add((int)item.User_id);

            foreach (var item in IDs)
                users.Add(App.db.Users.Where(x => x.ID == item).FirstOrDefault());

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
