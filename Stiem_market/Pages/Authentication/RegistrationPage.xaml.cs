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
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        AuthenticationViewModel viewModel;
        public RegistrationPage(AuthenticationViewModel _viewModel)
        {
            InitializeComponent();
            viewModel = _viewModel;
        }

        private void RegistAccount_Click(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == "" || PasswordTextBox.Password == "" || NickTextBox.Text == "")
                ErrorText.Text = "Заполните поля";
            else
            {
                // TODO: Логин не должен повторяться
                App.db.Users.Add(new Users()
                {
                    Nickname = NickTextBox.Text,
                    Email = LoginTextBox.Text,
                    Password = PasswordTextBox.Password,
                    IsDeleted = false,
                    IsDev = false,
                });
                App.db.SaveChanges();

                //NavigationService.Navigate(new UserProfile(viewModel.LoggedUser));
            }
        }
    }
}
