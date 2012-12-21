using System.Configuration;

namespace nfl_outlast.Utility
{
    public class Settings
    {
        public static string EspnFeedUrl { get { return ConfigurationManager.AppSettings["EspnFeedUrl"]; } }
 
    }
}