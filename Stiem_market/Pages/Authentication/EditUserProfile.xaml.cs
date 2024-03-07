using Microsoft.Win32;
using Stiem_market.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Stiem_market.Pages.Authentication
{
    /// <summary>
    /// Логика взаимодействия для EditUserProfile.xaml
    /// </summary>
    public partial class EditUserProfile : Page
    {
        private OpenFileDialog openFileDialog;
        private AuthenticationViewModel authViewModel;

        public EditUserProfile(AuthenticationViewModel _authViewModel)
        {
            InitializeComponent();

            authViewModel = _authViewModel;
            DataContext = authViewModel;

            SaveChanges.IsEnabled = false;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog != null)
            {
                if (openFileDialog.FileName != "")
                {
                    App.db.Users.Where(x =>
                        x.ID == authViewModel.LoggedUser.ID).FirstOrDefault().Avatar = File.ReadAllBytes(openFileDialog.FileName);
                }
            }

            App.db.Users.Where(x =>
                    x.ID == authViewModel.LoggedUser.ID).FirstOrDefault().Nickname = NicknameTextBox.Text;

            App.db.Users.Where(x =>
                    x.ID == authViewModel.LoggedUser.ID).FirstOrDefault().Description = DescriptionTextBox.Text;

            App.db.SaveChanges();

            authViewModel.LoggedUser = authViewModel.SelectedUser;

            NavigationService.GoBack();
        }

        private void SelectAPicture_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog = new OpenFileDialog()
            {
                Filter = "*.png|*.png|*.jpeg|*.jpeg|*.jpg|*.jpg|All files|*.*",
            };
            if (openFileDialog.ShowDialog().GetValueOrDefault())
            {
                ProfileImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                SaveChanges.IsEnabled = true;
            }
        }

        private void DescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DescriptionTextBox.Text == "")
                SaveChanges.IsEnabled = false;
            else
                SaveChanges.IsEnabled = true;
        }

        private void NicknameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NicknameTextBox.Text == "")
                SaveChanges.IsEnabled = false;
            else
                SaveChanges.IsEnabled = true;
        }
    }
}
