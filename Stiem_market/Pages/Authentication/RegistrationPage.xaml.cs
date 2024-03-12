using Stiem_market.Data;
using Stiem_market.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        AuthenticationViewModel authViewModel;
        private string error;
        public RegistrationPage(AuthenticationViewModel _authViewModel)
        {
            InitializeComponent();
            authViewModel = _authViewModel;
        }

        private void RegistAccount_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(LoginTextBox.Text) || string.IsNullOrEmpty(PasswordTextBox.Password) || string.IsNullOrEmpty(LoginTextBox.Text))
                error = "Заполните поля";
            else
            {
                if (ErrorText.Text == "")
                {
                    App.db.Users.Add(new Users()
                    {
                        Nickname = NickTextBox.Text,
                        Email = LoginTextBox.Text,
                        Password = PasswordTextBox.Password,
                        IsDev = false,
                        IsDeleted = false,
                        Description = "",
                        Balance = 0,
                    });
                    App.db.SaveChanges();

                    authViewModel.UserCollection = App.db.Users.ToList();
                    authViewModel.LoggedUser = authViewModel.UserCollection.Where(u => u.Email == LoginTextBox.Text).FirstOrDefault();
                    authViewModel.SelectedUser = authViewModel.LoggedUser;

                    NavigationService.Navigate(new UserProfile(false, authViewModel));
                }
            }

            ErrorText.Text = error;
        }

        private void LoginTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            EmailAddressAttribute email = new EmailAddressAttribute();
            error = "";

            if (!email.IsValid(LoginTextBox.Text))
            {
                error = "Неверная почта";
            }
            else
            {
                if (App.db.Users.Any(u => u.Email == LoginTextBox.Text))
                {
                    error = "Почта уже используется";
                }
            }
            ErrorText.Text = error;
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}
