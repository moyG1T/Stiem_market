using Stiem.ViewModels;
using Stiem_market.Data;
using Stiem_market.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

        private string greeting;
        public string Greeting
        {
            get
            {
                Random random = new Random();
                bool isGreetings = random.Next(1, 10) > 4;

                if (isGreetings)
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


        private void GameList_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = null;
            if (sender.Equals(FirstGameList))
            {
                listBox = FirstGameList;
            }
            else if (sender.Equals(SecondGameList))
            {
                listBox = SecondGameList;
            }
            else if (sender.Equals(ThirdGameList))
            {
                listBox = ThirdGameList;
            }

            if (listBox.SelectedItem == null)
                return;

            gameViewModel.SelectedGame = listBox.SelectedItem as Games;
            NavigationService.Navigate(new GamePage(true, authViewModel, gameViewModel));
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
    }
}
