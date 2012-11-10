namespace nfl_outlast.tests.Feeds
{
    public class EspnFeedGame
    {
        public EspnFeedTeam AwayTeam { get; set; }
        public EspnFeedTeam HomeTeam { get; set; }
        public GameStatus Status { get; set; }
        public string GameUrl { get; set; }
        public string Time { get; set; }

        public EspnFeedGame(EspnFeedTeam awayTeam, EspnFeedTeam homeTeam, GameStatus status)
        {
            AwayTeam = awayTeam;
            HomeTeam = homeTeam;
            Status = status;
        }
    }
}