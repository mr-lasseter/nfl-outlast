using nfl_outlast.Shared;

namespace nfl_outlast.Feeds
{
    public class EspnFeedParser
    {
        private readonly EspnGameParser _gameParser;

        public EspnFeedParser()
        {
            _gameParser = new EspnGameParser();
        }

        public EspnFeed Parse(string rawFeed)
        {
            var feed = new EspnFeed();

            rawFeed = rawFeed.Replace("%20", " ");
            rawFeed = rawFeed.Replace("^", "");
            var games = rawFeed.Split("nfl_s_left");

            for (var i = 1; i < games.Length; i++)
            {
                var game = _gameParser.ParseGame(games[i]);
                feed.AddGame(game);
            }

            return feed;
        }
    }
}