using Stiem_market.ViewModels;
using System.Windows.Controls;

namespace Stiem_market.Pages.Store
{
    /// <summary>
    /// Логика взаимодействия для PurchaseHistoryPage.xaml
    /// </summary>
    public partial class PurchaseHistoryPage : Page
    {
        private bool canNavigateBack;
        private AuthenticationViewModel authViewModel;
        public PurchaseHistoryPage(bool _canNavigateBack, AuthenticationViewModel _authViewModel)
        {
            InitializeComponent();

            canNavigateBack = _canNavigateBack;
            authViewModel = _authViewModel;

            DataContext = authViewModel;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}
