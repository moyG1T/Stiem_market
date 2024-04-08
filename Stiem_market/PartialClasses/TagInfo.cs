using Stiem_market.Data;
using System.Collections.Generic;
using System.Linq;

namespace Stiem_market.PartialClasses
{
    public class TagInfo
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public IEnumerable<Games> ContainingGames
        {
            get => App.db.Games.Where(g => g.GameTags.Any(gt => gt.Game_id == g.ID && gt.Tags.Title == Title)).Take(16).ToList();
        }
    }
}
