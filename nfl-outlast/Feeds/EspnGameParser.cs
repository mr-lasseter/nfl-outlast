using nfl_outlast.Shared;

namespace nfl_outlast.Feeds
{
    public interface IEspnGameParser
    {
        bool CanParse(string gameDetails);
        EspnFeedGame ParseGame(string game);
    }

    public abstract class EspnGameParser : IEspnGameParser
    {
        protected abstract bool CanParseGameDetails(string gameDetails);
        protected abstract EspnFeedGame ParseGameDetails(string gameDetails);

        public bool CanParse(string game)
        {
            return CanParseGameDetails(CleanUpGameDetails(game));
        }

        public EspnFeedGame ParseGame(string game)
        {
            var gameDetails = CleanUpGameDetails(game);

            var feedGame = ParseGameDetails(gameDetails);

            var rawDataUrl = game.Split("nfl_s_url")[1];
            var startIndex = rawDataUrl.Index("=") + 1;
            var url = rawDataUrl.Substring(startIndex, rawDataUrl.Length - startIndex - 1);
            feedGame.GameUrl = url;

            return feedGame;
        }

        private string CleanUpGameDetails(string game)
        {
            var firstEqualSign = game.Index("=") + 1;
            var firstAndSign = game.Index("&");
            return game.Substring(firstEqualSign, firstAndSign - firstEqualSign);
        }
    }
}