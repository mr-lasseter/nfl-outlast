using System.Collections.Generic;
using NUnit.Framework;
using System.Linq;
using nfl_outlast.Shared;

namespace nfl_outlast.tests.Feeds
{
    public class EspnFeedParserSpecs
    {
        [TestFixture]
        public class When_asked_to_parse_the_feed
        {
            //this is pulled from http://sports.espn.go.com/nfl/bottomline/scores
            private const string notInGameFeed =
                @"&nfl_s_delay=120&nfl_s_stamp=1109054058&nfl_s_left1=^Indianapolis%2027%20%20%20Jacksonville%2010%20(FINAL)&nfl_s_right1_count=0&nfl_s_url1=http://sports.espn.go.com/nfl/boxscore?gameId=321108030&nfl_s_left2=NY%20Giants%20at%20Cincinnati%20(1:00%20PM%20ET)&nfl_s_right2_count=0&nfl_s_url2=http://sports.espn.go.com/nfl/preview?gameId=321111004&nfl_s_left3=Tennessee%20at%20Miami%20(1:00%20PM%20ET)&nfl_s_right3_count=0&nfl_s_url3=http://sports.espn.go.com/nfl/preview?gameId=321111015&nfl_s_left4=Detroit%20at%20Minnesota%20(1:00%20PM%20ET)&nfl_s_right4_count=0&nfl_s_url4=http://sports.espn.go.com/nfl/preview?gameId=321111016&nfl_s_left5=Buffalo%20at%20New%20England%20(1:00%20PM%20ET)&nfl_s_right5_count=0&nfl_s_url5=http://sports.espn.go.com/nfl/preview?gameId=321111017&nfl_s_left6=Atlanta%20at%20New%20Orleans%20(1:00%20PM%20ET)&nfl_s_right6_count=0&nfl_s_url6=http://sports.espn.go.com/nfl/preview?gameId=321111018&nfl_s_left7=San%20Diego%20at%20Tampa%20Bay%20(1:00%20PM%20ET)&nfl_s_right7_count=0&nfl_s_url7=http://sports.espn.go.com/nfl/preview?gameId=321111027&nfl_s_left8=Denver%20at%20Carolina%20(1:00%20PM%20ET)&nfl_s_right8_count=0&nfl_s_url8=http://sports.espn.go.com/nfl/preview?gameId=321111029&nfl_s_left9=Oakland%20at%20Baltimore%20(1:00%20PM%20ET)&nfl_s_right9_count=0&nfl_s_url9=http://sports.espn.go.com/nfl/preview?gameId=321111033&nfl_s_left10=NY%20Jets%20at%20Seattle%20(4:05%20PM%20ET)&nfl_s_right10_count=0&nfl_s_url10=http://sports.espn.go.com/nfl/preview?gameId=321111026&nfl_s_left11=Dallas%20at%20Philadelphia%20(4:25%20PM%20ET)&nfl_s_right11_count=0&nfl_s_url11=http://sports.espn.go.com/nfl/preview?gameId=321111021&nfl_s_left12=St.%20Louis%20at%20San%20Francisco%20(4:25%20PM%20ET)&nfl_s_right12_count=0&nfl_s_url12=http://sports.espn.go.com/nfl/preview?gameId=321111025&nfl_s_left13=Houston%20at%20Chicago%20(8:20%20PM%20ET)&nfl_s_right13_count=0&nfl_s_url13=http://sports.espn.go.com/nfl/preview?gameId=321111003&nfl_s_left14=Kansas%20City%20at%20Pittsburgh%20(8:30%20PM%20ET)&nfl_s_right14_count=0&nfl_s_url14=http://sports.espn.go.com/nfl/preview?gameId=321112023&nfl_s_count=14&nfl_s_loaded=true";

            [Test]
            public void Should_be_able_to_provide_a_listing_of_teams_playing()
            {
                var feed = ExecuteSUT();

                feed.GameCount().ShouldEqual(14);
            }

            [Test]
            public void Should_parse_home_vs_away_for_a_game_that_is_complete()
            {
                var feed = ExecuteSUT();

                var game = feed.Games.First();

                game.HomeTeam.Name.ShouldEqual("Jacksonville");
                game.HomeTeam.Score.ShouldEqual(10);
                game.AwayTeam.Name.ShouldEqual("Indianapolis");
                game.AwayTeam.Score.ShouldEqual(27);
            }
        

            private EspnFeed ExecuteSUT()
            {
                return new EspnFeedParser().Parse(notInGameFeed);
            }
        }
    }

    public class EspnFeedParser
    {
        public EspnFeed Parse(string rawFeed)
        {
            var feed = new EspnFeed();

            rawFeed = rawFeed.Replace("%20", " ");
            rawFeed = rawFeed.Replace("^", "");
            var games = rawFeed.Split("nfl_s_left");

            for (int i = 1; i < games.Length; i++)
            {
                var firstEqualSign = games[i].Index("=") + 1;
                var firstAndSign = games[i].Index("&");
                var gameDetails = games[i].Substring(firstEqualSign, firstAndSign - firstEqualSign);

                if (gameDetails.Contains("(FINAL)"))
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

                    feed.AddGame(new EspnFeedGame(homeTeam, awayTeam));

                }
                else
                {
                    feed.AddGame(new EspnFeedGame(new EspnFeedTeam(), new EspnFeedTeam()));
                }
            }

            return feed;
        }
    }

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

    public class EspnFeedGame
    {
        public EspnFeedTeam HomeTeam { get; set; }
        public EspnFeedTeam AwayTeam { get; set; }

        public EspnFeedGame(EspnFeedTeam homeTeam, EspnFeedTeam awayTeam)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
        }
    }

    public class EspnFeedTeam
    {
        public string Name { get; set; }
        public int Score { get; set; }
    }
}