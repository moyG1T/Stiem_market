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

namespace Stiem_market.Pages.Library
{
    /// <summary>
    /// Логика взаимодействия для LibraryPage.xaml
    /// </summary>
    public partial class LibraryPage : Page
    {
        private AuthenticationViewModel authVievModel;

        public LibraryPage(bool _canNavigateBack, AuthenticationViewModel _authViewModel)
        {
            InitializeComponent();

            authVievModel = _authViewModel;
            DataContext = authVievModel;
        }
    }
}
