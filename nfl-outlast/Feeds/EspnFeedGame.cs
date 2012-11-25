using System;

namespace nfl_outlast.Feeds
{
    public class EspnFeedGame
    {
        public EspnFeedTeam AwayTeam { get; set; }
        public EspnFeedTeam HomeTeam { get; set; }
        public GameStatus Status { get; set; }
        public string GameUrl { get; set; }
        public string KickoffTime { get; set; }
        public int CurrentQuarter { get; set; }
        public TimeSpan? TimeLeftInQuarter { get; set; }

        public EspnFeedGame(EspnFeedTeam awayTeam, EspnFeedTeam homeTeam, GameStatus status)
        {
            AwayTeam = awayTeam;
            HomeTeam = homeTeam;
            Status = status;
        }
    }
}