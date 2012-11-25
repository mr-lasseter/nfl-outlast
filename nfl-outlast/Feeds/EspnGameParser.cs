using System;
using nfl_outlast.Shared;
using System.Linq;

namespace nfl_outlast.Feeds
{
    public class EspnGameParser
    {
        public EspnFeedGame ParseGame(string game)
        {
            var firstEqualSign = game.Index("=") + 1;
            var firstAndSign = game.Index("&");
            var gameDetails = game.Substring(firstEqualSign, firstAndSign - firstEqualSign);

            EspnFeedGame feedGame = null;
            if (gameDetails.Contains("(FINAL)") || gameDetails.Contains("(FINAL - OT)"))
            {
                feedGame = ParseFinishedGame(gameDetails);
            }
            else if (gameDetails.Contains(" at "))
            {
                feedGame = ParseNotStartedGame(gameDetails);
            }
            else if (gameDetails.Contains(" IN "))
            {
                feedGame = ParseInProgressGame(gameDetails);
            }

            var rawDataUrl = game.Split("nfl_s_url")[1];
            var startIndex = rawDataUrl.Index("=") + 1;
            var url = rawDataUrl.Substring(startIndex, rawDataUrl.Length - startIndex - 1);
            feedGame.GameUrl = url;

            return feedGame;
        }

        private EspnFeedGame ParseInProgressGame(string gameDetails)
        {
            var lastClosedParen = gameDetails.LastIndexOf(")");
            var lastOpenParen = gameDetails.LastIndexOf("(");

            var timeLeftOnClock = gameDetails.Substring(lastOpenParen, lastClosedParen - lastOpenParen + 1);
            var scoreDetails = gameDetails.Replace(timeLeftOnClock, "").Trim();
            var teamScoreDetails = scoreDetails.Split("   ");

            var lastSpace = teamScoreDetails[0].LastIndexOf(' ');
            var awayTeam = new EspnFeedTeam
            {
                Name = teamScoreDetails[0].Substring(0, lastSpace),
                Score = int.Parse(teamScoreDetails[0].Substring(lastSpace))
            };

            lastSpace = teamScoreDetails[1].LastIndexOf(' ');
            var homeTeam = new EspnFeedTeam
            {
                Name = teamScoreDetails[1].Substring(0, lastSpace),
                Score = int.Parse(teamScoreDetails[1].Substring(lastSpace))
            };

            var timeAndQuarter = timeLeftOnClock.Split(" IN ");
            var timeLeft = new string(timeAndQuarter[0].Where(char.IsNumber).ToArray());
            var minutesLeft = int.Parse(timeLeft.Substring(0, 2));
            var secondsLeft = int.Parse(timeLeft.Substring(2, 2));
            var quarter = int.Parse(new string(timeAndQuarter[1].Where(char.IsNumber).ToArray()));

            var game = new EspnFeedGame(awayTeam, homeTeam, GameStatus.InProgress);
            game.TimeLeftInQuarter = new TimeSpan(0, minutesLeft, secondsLeft);
            game.CurrentQuarter = quarter;
            return game;

        }

        private EspnFeedGame ParseNotStartedGame(string gameDetails)
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

        private EspnFeedGame ParseFinishedGame(string gameDetails)
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