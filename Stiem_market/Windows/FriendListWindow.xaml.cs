using Stiem_market.Data;
using Stiem_market.ViewModels;
using System.Windows;

namespace Stiem_market.Windows
{
    /// <summary>
    /// Логика взаимодействия для FriendListWindow.xaml
    /// </summary>
    public partial class FriendListWindow : Window
    {
        private AuthenticationViewModel authViewModel;
        public FriendListWindow(AuthenticationViewModel _authViewModel)
        {
            InitializeComponent();

            authViewModel = _authViewModel;
            DataContext = authViewModel;
        }

        private void ListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            authViewModel.PayCart(FriendList.SelectedItem as Users);
            SuccessPopup.IsOpen = true;
            Close();
        }
    }
}
