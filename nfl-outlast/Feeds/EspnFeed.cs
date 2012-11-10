using System.Collections.Generic;
using System.Linq;

namespace nfl_outlast.tests.Feeds
{
    public class EspnFeed
    {
        public IList<EspnFeedGame> Games { get; set; }

        public EspnFeed()
        {
            Games = new List<EspnFeedGame>();
        }

        public int GameCount()
        {
            return Games.Count();
        }

        public void AddGame(EspnFeedGame game)
        {
            Games.Add(game);
        }
       
    }
}