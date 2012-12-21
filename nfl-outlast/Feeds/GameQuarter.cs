using System;

namespace nfl_outlast.Feeds
{
    public enum GameQuarter
    {
        First,
        Second,
        Third,
        Fourth,
        Overtime
    }

    public static class GameQuarterParser
    {
        public static GameQuarter ParseQuarter(this string quarter)
        {
            string cleanedQuarterDescription = quarter.Trim().ToUpper();
            switch (cleanedQuarterDescription)
            {
                case "1ST": return GameQuarter.First;
                case "2ND": return GameQuarter.Second;
                case "3RD": return GameQuarter.Third;
                case "4TH": return GameQuarter.Fourth;
                case "OT": return GameQuarter.Overtime;
            }
            throw new InvalidOperationException("Unable to parse quarter " + cleanedQuarterDescription);
        }
    }
}