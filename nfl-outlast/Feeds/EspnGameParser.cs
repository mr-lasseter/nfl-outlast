using nfl_outlast.Shared;

namespace nfl_outlast.Feeds
{
    public class EspnGameParser
    {
        public EspnFeedGame ParseGame(string game)
        {
            var firstEqualSign = game.Index("=") + 1;
            var firstAndSign = game.Index("&");
            var gameDetails = game.Substring(firstEqualSign, firstAndSign - firstEqualSign);

            EspnFeedGame feedGame;
            if (gameDetails.Contains("(FINAL)"))
            {
                feedGame = ParseFinishedGames(gameDetails);
            }
            else if (gameDetails.Contains(" at "))
            {
                feedGame = ParseNotStartedGames(gameDetails);
            }
            else
            {
                //TODO:  Figure out what the feed looks like for during game
                feedGame = new EspnFeedGame(new EspnFeedTeam(), new EspnFeedTeam(), GameStatus.InProgress);
            }

            var rawDataUrl = game.Split("nfl_s_url")[1];
            var startIndex = rawDataUrl.Index("=") + 1;
            var url = rawDataUrl.Substring(startIndex, rawDataUrl.Length - startIndex - 1);
            feedGame.GameUrl = url;

            return feedGame;
        }

        private EspnFeedGame ParseNotStartedGames(string gameDetails)
        {
            var firstParen = gameDetails.Index("(");
            var rawGameDetails = gameDetails.Substring(0, firstParen).Trim();
            var rawTeamDetails = rawGameDetails.Split(" at ");
            var awayTeam = new EspnFeedTeam { Name = rawTeamDetails[0] };
            var homeTeam = new EspnFeedTeam { Name = rawTeamDetails[1] };

            var timeDetails = gameDetails.Substring(firstParen, gameDetails.Length - firstParen);
            var time = timeDetails.Trim('(').Trim(')');

            var game = new EspnFeedGame(awayTeam, homeTeam, GameStatus.NotStarted);
            game.Time = time;
            return game;
        }

        private EspnFeedGame ParseFinishedGames(string gameDetails)
        {
            var rawGameDetails = gameDetails.Replace(" (FINAL)", "");
            var rawTeamDetails = rawGameDetails.Split("   ");
            var lastSpace = rawTeamDetails[0].LastIndexOf(' ');
            var awayTeam = new EspnFeedTeam
                {
                    Name = rawTeamDetails[0].Substring(0, lastSpace),
                    Score = int.Parse(rawTeamDetails[0].Substring(lastSpace))
                };

            var homeTeam = new EspnFeedTeam
                {
                    Name = rawTeamDetails[1].Substring(0, lastSpace),
                    Score = int.Parse(rawTeamDetails[1].Substring(lastSpace))
                };

            return new EspnFeedGame(awayTeam, homeTeam, GameStatus.Finished);
        }
    }
}