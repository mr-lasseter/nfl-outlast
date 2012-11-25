using System.IO;
using System.Net;
using nfl_outlast.Utility;

namespace nfl_outlast.Feeds
{
    public class EspnFeedFetcher
    {
        private readonly EspnFeedParser _parser;
        public EspnFeedFetcher()
        {
            _parser = new EspnFeedParser();
        }

        public EspnFeed Fetch()
        {
            var request = WebRequest.Create(Settings.EspnFeedUrl);
            request.Method = "GET";

            var responseString = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
            return _parser.Parse(responseString);
        }
    }
}