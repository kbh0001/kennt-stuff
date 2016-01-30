using System.Collections.Generic;

namespace NinjaTrader.Custom.Strategy
{
    public class MoveBinaryStrikeWidthProvider
    {
        private static readonly SortedList<string, double> _binaryWidths = new SortedList<string, double>()
        {
            {"$AUDJPY", .05},
            {"$AUDUSD", .0005},
            {"$EURGBP", .00010},
            {"$EURJPY", .1},
            {"$EURUSD", .0004},
            {"$GBPJPY", .1},
            {"$GBPUSD", .0010},
            {"$USDCAD", .0010},
            {"$USDCHF", .0004},
            {"$USDJPY", .04},
        };



        public static double GetBinaryStrikeWidthFor(string insturment)
        {
         

            return _binaryWidths[insturment];

        }
        
    }
}