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

namespace Stiem_market.Pages.Authentication
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        AuthenticationViewModel viewModel;
        public AuthorizationPage(bool _canNavigateBack, AuthenticationViewModel _viewModel)
        {
            InitializeComponent();

            viewModel = _viewModel;

            LoginTextBox.Focus();
        }

        private void LoginAccount_Click(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "" && PasswordTextBox.Password == "")
                ErrorText.Text = "Заполните логин и пароль";
            else if (LoginTextBox.Text == "")
                ErrorText.Text = "Заполните логин";
            else if (PasswordTextBox.Password == "")
                ErrorText.Text = "Заполните пароль";
            else
            {
                Users user = App.db.Users.Where(x => x.Email == LoginTextBox.Text && x.Password == PasswordTextBox.Password).FirstOrDefault();
                if (user == null)
                {
                    ErrorText.Text = "Неверные данные";
                }
                else
                {
                    viewModel.SelectedUser = user;
                    viewModel.LoggedUser = user;
                    NavigationService.Navigate(new UserProfile(false, viewModel));
                }
            }
        }

        private void RegistAccount_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RegistrationPage(viewModel));
        }
    }
}
