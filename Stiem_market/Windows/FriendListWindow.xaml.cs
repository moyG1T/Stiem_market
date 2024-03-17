using Stiem_market.Data;
using Stiem_market.ViewModels;
using System.Windows;
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
            authViewModel.PayCart(FriendList.SelectedItem as Users);
            SuccessPopup.IsOpen = true;
            Close();
        }
    }
}
