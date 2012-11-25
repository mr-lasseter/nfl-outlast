using nfl_outlast.Shared;

namespace nfl_outlast.Feeds
{
    public class EspnNotStartedGameParser : EspnGameParser
    {
        protected override bool CanParseGameDetails(string gameDetails)
        {
            return gameDetails.Contains(" at ");
        }

        protected override EspnFeedGame ParseGameDetails(string gameDetails)
        {
            var firstParen = gameDetails.Index("(");
            var rawGameDetails = gameDetails.Substring(0, firstParen).Trim();
            var rawTeamDetails = rawGameDetails.Split(" at ");
            var awayTeam = new EspnFeedTeam { Name = rawTeamDetails[0] };
            var homeTeam = new EspnFeedTeam { Name = rawTeamDetails[1] };

            var timeDetails = gameDetails.Substring(firstParen, gameDetails.Length - firstParen);
            var time = timeDetails.Trim('(').Trim(')');

            var game = new EspnFeedGame(awayTeam, homeTeam, GameStatus.NotStarted);
            game.KickoffTime = time;
            return game;
        }
    }
}