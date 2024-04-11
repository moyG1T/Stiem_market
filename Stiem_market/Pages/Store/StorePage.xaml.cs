using Stiem.ViewModels;
using Stiem_market.Data;
using Stiem_market.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Stiem_market.Pages.Store
{
    /// <summary>
    /// Логика взаимодействия для StorePage.xaml
    /// </summary>
    public partial class StorePage : Page
    {
        private GameViewModel gameViewModel;
        private AuthenticationViewModel authViewModel;

        private bool isTuneClosed;

        private string greeting;
        public string Greeting
        {
            get
            {
                Random random = new Random();
                bool isGreetings = random.Next(1, 10) > 4;

                if (isGreetings || string.IsNullOrEmpty(greeting))
                {
                    string timeOfDay = "Доброго времени суток";
                    DateTime currentTime = DateTime.Now;
                    if (currentTime.Hour >= 5 && currentTime.Hour < 12)
                        timeOfDay = "Доброе утро";
                    else if (currentTime.Hour >= 12 && currentTime.Hour < 18)
                        timeOfDay = "Добрый день";
                    else if (currentTime.Hour >= 18 && currentTime.Hour < 23)
                        timeOfDay = "Добрый вечер";
                    else if (currentTime.Hour >= 23 && currentTime.Hour < 5)
                        timeOfDay = "Доброй ночи";

                    return $"{timeOfDay}, {greeting}";
                }
                else
                {
                    List<string> strings = new List<string> { "Ищите новинки?", "Не знаете во что поиграть?", "Игры для всех!" };
                    return strings.ElementAt(random.Next(strings.Count() - 1));
                }
            }
        }

        public StorePage(bool _canNavigateBack, AuthenticationViewModel _authViewModel)
        {
            InitializeComponent();
            isTuneClosed = false;
            CancelButton.Visibility = string.IsNullOrEmpty(Searchbar.Text) ? Visibility.Hidden : Visibility.Visible;

            authViewModel = _authViewModel;
            gameViewModel = new GameViewModel(authViewModel.FavoriteTags);
            DataContext = gameViewModel;

            greeting = authViewModel.LoggedUser.Nickname;
            GreetingText.DataContext = this;
        }

        private void ScrollViews_Click(object sender, RoutedEventArgs e)
        {
            ScrollViewer selectedScrollViewer = null;
            bool isPlus;

            if (sender == FirstMinus || sender == FirstPlus)
                selectedScrollViewer = FirstScrollViewer;
            else if (sender == SecondMinus || sender == SecondPlus)
                selectedScrollViewer = SecondScrollViewer;
            else if (sender == ThirdMinus || sender == ThirdPlus)
                selectedScrollViewer = ThirdScrollViewer;

            isPlus = sender == FirstPlus || sender == SecondPlus || sender == ThirdPlus;

            if (selectedScrollViewer != null)
            {
                double offsetChange = 870;
                if (!isPlus)
                {
                    offsetChange = -offsetChange;
                }

                if (selectedScrollViewer.HorizontalOffset == 0 && !isPlus)
                {
                    selectedScrollViewer.ScrollToRightEnd();
                }
                else if (selectedScrollViewer.HorizontalOffset + offsetChange == selectedScrollViewer.ExtentWidth)
                {
                    selectedScrollViewer.ScrollToHorizontalOffset(0);
                }
                else
                {
                    selectedScrollViewer.ScrollToHorizontalOffset(selectedScrollViewer.HorizontalOffset + offsetChange);
                }
            }
        }

        private void InnerScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                MainScrollViewer.ScrollToVerticalOffset(MainScrollViewer.VerticalOffset - SystemParameters.WheelScrollLines * 16);
            }
            else
            {
                MainScrollViewer.ScrollToVerticalOffset(MainScrollViewer.VerticalOffset + SystemParameters.WheelScrollLines * 16);
            }
        }

        private void ComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            // из-за поиска ломается бекспейс!!!
            if (string.IsNullOrWhiteSpace(Searchbar.Text))
            {
                Searchbar.ItemsSource = gameViewModel.Games;
            }
            else
            {
                string searchText = Searchbar.Text.ToLower();
                Searchbar.ItemsSource = gameViewModel.Games.Where(game => game.Title.ToLower().Contains(searchText)).ToList();
                Searchbar.IsDropDownOpen = true;
            }

            CancelButton.Visibility = string.IsNullOrEmpty(Searchbar.Text) ? Visibility.Hidden : Visibility.Visible;

            if (e.Key == Key.Enter)
            {
                if (Searchbar.SelectedItem != null)
                {
                    gameViewModel.SelectedGame = Searchbar.SelectedItem as Games;
                    NavigationService.Navigate(new GamePage(true, authViewModel, gameViewModel));
                    Searchbar.SelectedItem = null;
                }

                e.Handled = true;
            }
        }

        private void Searchbar_GotFocus(object sender, RoutedEventArgs e)
        {
            Searchbar.IsDropDownOpen = true;
        }

        private void Searchbar_LostFocus(object sender, RoutedEventArgs e)
        {
            Searchbar.IsDropDownOpen = false;
        }

        private void TuneButton_Click(object sender, RoutedEventArgs e)
        {
            SearchRow.Height = isTuneClosed ? new GridLength(70, GridUnitType.Pixel) : new GridLength(200, GridUnitType.Pixel);
            DropFiltersButton_Click(null, null);
            isTuneClosed = !isTuneClosed;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Searchbar.Text = "";
            CancelButton.Visibility = Visibility.Hidden;
        }

        private void DropFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            TagsCombo.SelectedItem = null;
            DevsCombo.SelectedItem = null;
            SearchByFilters.IsEnabled = false;
        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchByFilters.IsEnabled = true;
        }

        private void SearchByFilters_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Games> list = App.db.Games.ToList();

            if (TagsCombo.SelectedItem != null && DevsCombo.SelectedItem != null)
            {
                list = list
                    .Where(l =>
                        l.Devs.ID == (DevsCombo.SelectedItem as Devs).ID
                        && l.GameTags.Any(gt => gt.Game_id == l.ID && gt.Tags.ID == (TagsCombo.SelectedItem as Tags).ID))
                    .ToList();
            }
            else if (TagsCombo.SelectedItem != null)
            {
                list = list.Where(l => l.GameTags.Any(gt => gt.Game_id == l.ID && gt.Tags.ID == (TagsCombo.SelectedItem as Tags).ID)).ToList();
            }
            else if (DevsCombo.SelectedItem != null)
            {
                list = list.Where(l => l.Devs.ID == (DevsCombo.SelectedItem as Devs).ID).ToList();
            }

            NavigationService.Navigate(new GameListPage(true, gameViewModel, list, TagsCombo.SelectedItem as Tags, DevsCombo.SelectedItem as Devs ));
        }

        private void ItemButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            gameViewModel.SelectedGame = (Games)button.DataContext;
            NavigationService.Navigate(new GamePage(true, authViewModel, gameViewModel));
        }
    }
}
