using nfl_outlast.Shared;
using System.Linq;

namespace nfl_outlast.Feeds
{
    public class EspnFeedParser
    {
        private readonly IEspnGameParser[] _gameParsers;

        public EspnFeedParser(IEspnGameParser[] gameParsers)
        {
            _gameParsers = gameParsers;
        }

        public EspnFeed Parse(string rawFeed)
        {
            var feed = new EspnFeed();

            rawFeed = rawFeed.Replace("%20", " ");
            rawFeed = rawFeed.Replace("^", "");
            var rawGames = rawFeed.Split("nfl_s_left");

            for (var i = 1; i < rawGames.Length; i++)
            {
                var gameDetails = rawGames[i];
                var game = _gameParsers
                            .First(x => x.CanParse(gameDetails))
                            .ParseGame(gameDetails);
                
                feed.AddGame(game);
            }

            return feed;
        }
    }
}