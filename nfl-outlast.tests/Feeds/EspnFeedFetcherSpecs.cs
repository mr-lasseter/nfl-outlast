using System;
using NUnit.Framework;
using nfl_outlast.Feeds;

namespace nfl_outlast.tests.Feeds
{
    public class EspnFeedFetcherSpecs
    {
        [TestFixture]
        public class When_asked_to_read_the_feed
        {
            [Test, Explicit]
            public void Should_pull_the_feed_from_espn()
            {
                var feedFetcher = new EspnFeedFetcher();
                var feed = feedFetcher.Fetch();
                feed.Games.Count.ShouldBeGreaterThan(0);
            }
        }
    }
}