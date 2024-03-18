using Stiem_market.Data;
using Stiem_market.ViewModels;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Stiem_market.Windows
{
    /// <summary>
    /// Логика взаимодействия для PurchasingForFriends.xaml
    /// </summary>
    public partial class FriendListWindow : Window
    {
        private AuthenticationViewModel authViewModel;
        private Games game;
        public FriendListWindow(AuthenticationViewModel _authViewModel, Games _game)
        {
            InitializeComponent();

            authViewModel = _authViewModel;
            game = _game;
            DataContext = authViewModel;
        }

        private void FriendList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FriendHasItAlready.IsOpen = false;
            NeedToConfirm.IsOpen = false;

            int id = (FriendList.SelectedItem as Users).ID;
            bool hasGame = App.db.GameInCarts.Any(g => App.db.Carts.Any(c => c.ID == g.Cart_id && c.Owner_id == id && g.Game_id == game.ID));
            if (hasGame)
            {
                FriendHasItAlready.IsOpen = true;
                return;
            }

            if (Confirmed.IsChecked == false)
            {
                NeedToConfirm.IsOpen = true;
                return;
            }

            App.db.Carts.Add(new Carts
            {
                Purchaser_id = authViewModel.LoggedUser.ID,
                RelationType = 4,
            });
            App.db.SaveChanges();

            App.db.GameInCarts.Add(new GameInCarts
            {
                Game_id = game.ID,
                Cart_id = App.db.Carts.Where(c => c.Purchaser_id == authViewModel.LoggedUser.ID && c.RelationType == 4).FirstOrDefault().ID,
            });
            App.db.SaveChanges();

            authViewModel.PayCart(FriendList.SelectedItem as Users);
            Close();
        }
    }
}
