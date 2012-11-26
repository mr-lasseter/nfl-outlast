using nfl_outlast.Shared;

namespace nfl_outlast.Feeds
{
    public class EspnFinalGameParser : EspnGameParser
    {
        protected override bool CanParseGameDetails(string gameDetails)
        {
            return gameDetails.Contains("(FINAL)") || gameDetails.Contains("(FINAL - OT)");
        }

        protected override EspnFeedGame ParseGameDetails(string gameDetails)
        {
            var rawGameDetails = gameDetails.Replace(" (FINAL)", "");
            rawGameDetails = rawGameDetails.Replace(" (FINAL - OT)", "");
            var rawTeamDetails = rawGameDetails.Split("   ");
            var lastSpace = rawTeamDetails[0].LastIndexOf(' ');
            var awayTeam = new EspnFeedTeam
                {
                    Name = rawTeamDetails[0].Substring(0, lastSpace),
                    Score = int.Parse(rawTeamDetails[0].Substring(lastSpace))
                };

            lastSpace = rawTeamDetails[1].LastIndexOf(' ');
            var homeTeam = new EspnFeedTeam
                {
                    Name = rawTeamDetails[1].Substring(0, lastSpace),
                    Score = int.Parse(rawTeamDetails[1].Substring(lastSpace))
                };

            var gameStatus = gameDetails.Contains("(FINAL)") ? GameStatus.Final : GameStatus.FinalInOvertime;
            return new EspnFeedGame(awayTeam, homeTeam, gameStatus);
        }
    }
}