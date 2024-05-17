using Stiem.ViewModels;
using Stiem_market.Data;
using Stiem_market.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Stiem_market.Pages.Store
{
    /// <summary>
    /// Логика взаимодействия для GameListPage.xaml
    /// </summary>
    public partial class GameListPage : Page, INotifyPropertyChanged
    {
        private GameViewModel gameViewModel;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private IEnumerable<Games> gameCollection;
        public IEnumerable<Games> GameCollection
        {
            get => gameCollection;
            set
            {
                gameCollection = value;
                OnPropertyChanged(nameof(GameCollection));
            }
        }

        private bool noResults = false;
        public bool NoResults
        {
            get => noResults;
            set
            {
                noResults = value;
                OnPropertyChanged(nameof(NoResults));
            }
        }

        private AuthenticationViewModel authenticationViewModel;
        public GameListPage(bool _canNavigateBack, AuthenticationViewModel _authenticationViewModel, GameViewModel _gameViewModel, IEnumerable<Games> gameList, Tags tag, Devs dev)
        {
            InitializeComponent();
            authenticationViewModel = _authenticationViewModel;
            gameViewModel = _gameViewModel;
            GameCollection = gameList;

            DataContext = gameViewModel;
            GameList.DataContext = this;
            NoResultText.DataContext = this;

            CancelButton.Visibility = _canNavigateBack ? Visibility.Visible : Visibility.Hidden;

            TagsCombo.SelectedItem = tag;
            DevsCombo.SelectedItem = dev;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void Filter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void DropFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            TagsCombo.SelectedItem = null;
            DevsCombo.SelectedItem = null;
            Filter();
        }

        private void SearchByFilters_Click(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private IQueryable<Games> FilterQuery()
        {
            IQueryable<Games> list = App.db.Games;

            int devId;
            int tagId;
            if (TagsCombo.SelectedItem != null && DevsCombo.SelectedItem != null)
            {
                devId = (DevsCombo.SelectedItem as Devs).ID;
                tagId = (TagsCombo.SelectedItem as Tags).ID;
                list = list
                    .Where(l =>
                        l.Devs.ID == devId && l.GameTags.Any(gt => gt.Game_id == l.ID && gt.Tags.ID == tagId));
            }
            else if (TagsCombo.SelectedItem != null)
            {
                tagId = (TagsCombo.SelectedItem as Tags).ID;
                list = list.Where(l => l.GameTags.Any(gt => gt.Game_id == l.ID && gt.Tags.ID == tagId));
            }
            else if (DevsCombo.SelectedItem != null)
            {
                devId = (DevsCombo.SelectedItem as Devs).ID;
                list = list.Where(l => l.Devs.ID == devId);
            }

            if (!string.IsNullOrEmpty(SearchBar.Text))
            {
                list = list.Where(l => l.Title.ToLower().Contains(SearchBar.Text.ToLower()));
            }

            return list;
        }

        private void Filter()
        {
            IQueryable<Games> query = FilterQuery();
            GameCollection = query.ToList();
            NoResults = GameCollection.Count() == 0;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SearchBar.Text = "";
            Filter();
        }

        private void GameList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (GameList.SelectedItem as Games != null)
            {
                gameViewModel.SelectedGame = GameList.SelectedItem as Games;
                NavigationService.Navigate(new GamePage(true, authenticationViewModel, gameViewModel));
            }
        }
    }
}
