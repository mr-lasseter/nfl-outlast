using System.Linq;
using NUnit.Framework;
using nfl_outlast.Feeds;

namespace nfl_outlast.tests.Feeds
{
    public class EspnFeedParserSpecs
    {
        [TestFixture]
        public class When_asked_to_parse_the_feed_when_no_games_are_playing
        {
            //this is pulled from http://sports.espn.go.com/nfl/bottomline/scores
            private const string notInGameFeed =
                @"&nfl_s_delay=120&nfl_s_stamp=1109054058&nfl_s_left1=^Indianapolis%2027%20%20%20Jacksonville%2010%20(FINAL)&nfl_s_right1_count=0&nfl_s_url1=http://sports.espn.go.com/nfl/boxscore?gameId=321108030&nfl_s_left2=NY%20Giants%20at%20Cincinnati%20(1:00%20PM%20ET)&nfl_s_right2_count=0&nfl_s_url2=http://sports.espn.go.com/nfl/preview?gameId=321111004&nfl_s_left3=Tennessee%20at%20Miami%20(1:00%20PM%20ET)&nfl_s_right3_count=0&nfl_s_url3=http://sports.espn.go.com/nfl/preview?gameId=321111015&nfl_s_left4=Detroit%20at%20Minnesota%20(1:00%20PM%20ET)&nfl_s_right4_count=0&nfl_s_url4=http://sports.espn.go.com/nfl/preview?gameId=321111016&nfl_s_left5=Buffalo%20at%20New%20England%20(1:00%20PM%20ET)&nfl_s_right5_count=0&nfl_s_url5=http://sports.espn.go.com/nfl/preview?gameId=321111017&nfl_s_left6=Atlanta%20at%20New%20Orleans%20(1:00%20PM%20ET)&nfl_s_right6_count=0&nfl_s_url6=http://sports.espn.go.com/nfl/preview?gameId=321111018&nfl_s_left7=San%20Diego%20at%20Tampa%20Bay%20(1:00%20PM%20ET)&nfl_s_right7_count=0&nfl_s_url7=http://sports.espn.go.com/nfl/preview?gameId=321111027&nfl_s_left8=Denver%20at%20Carolina%20(1:00%20PM%20ET)&nfl_s_right8_count=0&nfl_s_url8=http://sports.espn.go.com/nfl/preview?gameId=321111029&nfl_s_left9=Oakland%20at%20Baltimore%20(1:00%20PM%20ET)&nfl_s_right9_count=0&nfl_s_url9=http://sports.espn.go.com/nfl/preview?gameId=321111033&nfl_s_left10=NY%20Jets%20at%20Seattle%20(4:05%20PM%20ET)&nfl_s_right10_count=0&nfl_s_url10=http://sports.espn.go.com/nfl/preview?gameId=321111026&nfl_s_left11=Dallas%20at%20Philadelphia%20(4:25%20PM%20ET)&nfl_s_right11_count=0&nfl_s_url11=http://sports.espn.go.com/nfl/preview?gameId=321111021&nfl_s_left12=St.%20Louis%20at%20San%20Francisco%20(4:25%20PM%20ET)&nfl_s_right12_count=0&nfl_s_url12=http://sports.espn.go.com/nfl/preview?gameId=321111025&nfl_s_left13=Houston%20at%20Chicago%20(8:20%20PM%20ET)&nfl_s_right13_count=0&nfl_s_url13=http://sports.espn.go.com/nfl/preview?gameId=321111003&nfl_s_left14=Kansas%20City%20at%20Pittsburgh%20(8:30%20PM%20ET)&nfl_s_right14_count=0&nfl_s_url14=http://sports.espn.go.com/nfl/preview?gameId=321112023&nfl_s_count=14&nfl_s_loaded=true";            

            [Test]
            public void Should_be_able_to_provide_a_listing_of_teams_playing()
            {
                var feed = ExecuteSUT();

                feed.Games.Count.ShouldEqual(14);
            }

            [Test]
            public void Should_parse_home_vs_away_for_a_game_that_is_complete()
            {
                var feed = ExecuteSUT();

                var game = feed.Games[0];

                game.AwayTeam.Name.ShouldEqual("Indianapolis");
                game.AwayTeam.Score.ShouldEqual(27);
                game.HomeTeam.Name.ShouldEqual("Jacksonville");
                game.HomeTeam.Score.ShouldEqual(10);

                game.Status.ShouldEqual(GameStatus.Final);
                game.GameUrl.ShouldEqual(@"http://sports.espn.go.com/nfl/boxscore?gameId=321108030");
            }

            [Test]
            public void Should_parse_home_vs_away_for_a_game_that_has_not_started()
            {
                var feed = ExecuteSUT();

                var game = feed.Games[1];

                game.AwayTeam.Name.ShouldEqual("NY Giants");
                game.AwayTeam.Score.ShouldEqual(0);
                game.HomeTeam.Name.ShouldEqual("Cincinnati");
                game.HomeTeam.Score.ShouldEqual(0);
                game.Time.ShouldEqual("1:00 PM ET");

                game.Status.ShouldEqual(GameStatus.NotStarted);
                game.GameUrl.ShouldEqual(@"http://sports.espn.go.com/nfl/preview?gameId=321111004");
            }

            private EspnFeed ExecuteSUT()
            {
                return new EspnFeedParser().Parse(notInGameFeed);
            }
        }

        [TestFixture]
        public class When_asked_to_parse_the_feed_when_games_are_playing
        {
            //this is pulled from http://sports.espn.go.com/nfl/bottomline/scores
            private const string inGameFeed =
                @"&nfl_s_delay=120&nfl_s_stamp=1125120445&nfl_s_left1=^Houston%2034%20%20%20Detroit%2031%20(FINAL%20-%20OT)&nfl_s_right1_count=0&nfl_s_url1=http://sports.espn.go.com/nfl/boxscore?gameId=321122008&nfl_s_left2=^Washington%2038%20%20%20Dallas%2031%20(FINAL)&nfl_s_right2_count=0&nfl_s_url2=http://sports.espn.go.com/nfl/boxscore?gameId=321122006&nfl_s_left3=^New%20England%2049%20%20%20NY%20Jets%2019%20(FINAL)&nfl_s_right3_count=0&nfl_s_url3=http://sports.espn.go.com/nfl/boxscore?gameId=321122020&nfl_s_left4=Minnesota%2016%20%20%20Chicago%2025%20(06:12%20IN%203RD)&nfl_s_right4_count=0&nfl_s_url4=http://sports.espn.go.com/nfl/boxscore?gameId=321125003&nfl_s_left5=Oakland%203%20%20%20Cincinnati%2024%20(07:04%20IN%203RD)&nfl_s_right5_count=0&nfl_s_url5=http://sports.espn.go.com/nfl/boxscore?gameId=321125004&nfl_s_left6=Pittsburgh%2014%20%20%20Cleveland%2013%20(07:05%20IN%203RD)&nfl_s_right6_count=0&nfl_s_url6=http://sports.espn.go.com/nfl/boxscore?gameId=321125005&nfl_s_left7=Buffalo%206%20%20%20Indianapolis%2013%20(10:14%20IN%203RD)&nfl_s_right7_count=0&nfl_s_url7=http://sports.espn.go.com/nfl/boxscore?gameId=321125011&nfl_s_left8=Denver%2014%20%20%20Kansas%20City%209%20(03:56%20IN%203RD)&nfl_s_right8_count=0&nfl_s_url8=http://sports.espn.go.com/nfl/boxscore?gameId=321125012&nfl_s_left9=Seattle%2014%20%20%20Miami%207%20(00:43%20IN%203RD)&nfl_s_right9_count=0&nfl_s_url9=http://sports.espn.go.com/nfl/boxscore?gameId=321125015&nfl_s_left10=Atlanta%2017%20%20%20Tampa%20Bay%2013%20(03:56%20IN%203RD)&nfl_s_right10_count=0&nfl_s_url10=http://sports.espn.go.com/nfl/boxscore?gameId=321125027&nfl_s_left11=Tennessee%209%20%20%20Jacksonville%2014%20(03:26%20IN%203RD)&nfl_s_right11_count=0&nfl_s_url11=http://sports.espn.go.com/nfl/boxscore?gameId=321125030&nfl_s_left12=Baltimore%20at%20San%20Diego%20(4:05%20PM%20ET)&nfl_s_right12_count=0&nfl_s_url12=http://sports.espn.go.com/nfl/preview?gameId=321125024&nfl_s_left13=San%20Francisco%20at%20New%20Orleans%20(4:25%20PM%20ET)&nfl_s_right13_count=0&nfl_s_url13=http://sports.espn.go.com/nfl/preview?gameId=321125018&nfl_s_left14=St.%20Louis%20at%20Arizona%20(4:25%20PM%20ET)&nfl_s_right14_count=0&nfl_s_url14=http://sports.espn.go.com/nfl/preview?gameId=321125022&nfl_s_left15=Green%20Bay%20at%20NY%20Giants%20(8:20%20PM%20ET)&nfl_s_right15_count=0&nfl_s_url15=http://sports.espn.go.com/nfl/preview?gameId=321125019&nfl_s_left16=Carolina%20at%20Philadelphia%20(8:30%20PM%20ET)&nfl_s_right16_count=0&nfl_s_url16=http://sports.espn.go.com/nfl/preview?gameId=321126021&nfl_s_count=16&nfl_s_loaded=true";

            [Test]
            public void Should_be_able_to_list_all_the_games()
            {
                var feed = ExecuteSUT();
                feed.Games.Count.ShouldEqual(16);
            }

            [Test]
            public void Should_be_able_to_parse_a_final_in_overtime()
            {
                var feed = ExecuteSUT();
                feed.Games.First().Status.ShouldEqual(GameStatus.FinalInOvertime);
            }

            private EspnFeed ExecuteSUT()
            {
                return new EspnFeedParser().Parse(inGameFeed);
            }
        }
    }
}