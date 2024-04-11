using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Stiem_market;
using Stiem_market.Data;
using Stiem_market.PartialClasses;
using static System.Collections.Specialized.NameObjectCollectionBase;

namespace Stiem.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private List<Tags> freeTags;
        private List<Tags> exceptedTags;
        private Random random;
        public GameViewModel(List<TagInfo> favTags)
        {
            Games = App.db.Games.ToList();
            TagsCollection = App.db.Tags.ToList();
            DevsCollection = App.db.Devs.ToList();

            // находим теги пользователя
            random = new Random();
            freeTags = App.db.Tags.ToList();
            exceptedTags = favTags.Count() > 0 ? favTags.Select(ft => new Tags { ID = ft.ID, Title = ft.Title }).ToList() : new List<Tags>();

            // если у пользователя недостаточно тегов, то будет выбран рандомный из существующих
            FirstFavGames = favTags.Count() > 0 ? favTags.ElementAt(0) : new TagInfo { Title = SelectRandomTag() };
            SecondFavGames = favTags.Count() > 1 ? favTags.ElementAt(1) : new TagInfo { Title = SelectRandomTag() };
            ThirdFavGames = favTags.Count() > 2 ? favTags.ElementAt(2) : new TagInfo { Title = SelectRandomTag() };
        }

        private string SelectRandomTag()
        {
            // исключаем теги пользователя из всех имеющихся
            freeTags = freeTags.Where(ft => !exceptedTags.Any(et => et.ID == ft.ID && et.Title == ft.Title)).ToList();
            // выбираем рандомный тег, которого нет у пользователя 
            Tags rndTag = freeTags.ElementAt(random.Next(freeTags.Count() - 1));
            // добавляем тег к исключениям
            exceptedTags.Add(rndTag);

            return rndTag.Title;
        }

        public TagInfo FirstFavGames { get; set; }
        public TagInfo SecondFavGames { get; set; }
        public TagInfo ThirdFavGames { get; set; }

        private Games selectedGame;
        public Games SelectedGame
        {
            get => selectedGame;
            set
            {
                selectedGame = value;
                OnPropertyChanged("SelectedGame");
            }
        }

        private IEnumerable<Games> games;
        public IEnumerable<Games> Games
        {
            get => games;
            set
            {
                games = value;
                OnPropertyChanged(nameof(Games));
            }
        }

        private IEnumerable<Tags> tagsCollection;
        public IEnumerable<Tags> TagsCollection
        {
            get => tagsCollection;
            set
            {
                tagsCollection = value;
                OnPropertyChanged(nameof(TagsCollection));
            }
        }

        private IEnumerable<Devs> devsCollection;
        public IEnumerable<Devs> DevsCollection
        {
            get => devsCollection;
            set
            {
                devsCollection = value;
                OnPropertyChanged(nameof(DevsCollection));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
