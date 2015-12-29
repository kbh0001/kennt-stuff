using System;

namespace NinjaTrader.Indicator
{
    public class KenCandleStickDeterminer
    {
        public static bool IsBearTrend(int trendStrength, bool isUpTrend)
        {
            return trendStrength > 0 && !isUpTrend;
        }

        public static bool IsBearishBeltHold(Indicator indicator, int trendstrenth, bool isUptrend)

        {
            if (indicator.CurrentBar < 1 || (IsBearTrend(trendstrenth, isUptrend)))
                return false;

            return indicator.Close[1] > indicator.Open[1] && indicator.Open[0] > indicator.Close[1] + 5 * indicator.TickSize && indicator.Open[0] == indicator.High[0] && indicator.Close[0] < indicator.Open[0];
        }

        public static bool IsBasicDoji(Indicator indicator)
        {
            return Math.Abs(indicator.Close[0] - indicator.Open[0]) <= (indicator.High[0] - indicator.Low[0]) * 0.07;
        }

        public static bool IsBearishEngulfing(Indicator indicator, int trendStrength, bool isUptrend)
        {
            if (indicator.CurrentBar < 1 || (IsBearTrend(trendStrength, isUptrend)))
                return false;

            return indicator.Close[1] > indicator.Open[1] && indicator.Close[0] < indicator.Open[0] && indicator.Open[0] > indicator.Close[1] && indicator.Close[0] < indicator.Open[1];
        }

        public static bool IsBearishHaramiCross(Indicator indicator, int trendWeight, bool isUptrend)
        {

            if (indicator.CurrentBar < 1 || (IsBearTrend(trendWeight, isUptrend)))
                return false;

            return (indicator.High[0] <= indicator.Close[1]) && (indicator.Low[0] >= indicator.Open[1]) && indicator.Open[0] <= indicator.Close[1] && indicator.Close[0] >= indicator.Open[1] &&
                   ((indicator.Close[0] >= indicator.Open[0] && indicator.Close[0] <= indicator.Open[0] + indicator.TickSize) ||
                    (indicator.Close[0] <= indicator.Open[0] && indicator.Close[0] >= indicator.Open[0] - indicator.TickSize));
        }

        public static bool IsBearishHarami(Indicator indicator, int trendWeigh, bool isUptrend)
        {
            if (indicator.CurrentBar < 1 || (IsBearTrend(trendWeigh, isUptrend)))
                return false;

            return indicator.Close[0] < indicator.Open[0] && indicator.Close[1] > indicator.Open[1] &&
                   indicator.Low[0] >= indicator.Open[1] && indicator.High[0] <= indicator.Close[1];
        }

        public static bool IsBullTrend(int trendWeight, bool isDownTrend)
        {
            return trendWeight > 0 && !isDownTrend;
        }

        public static bool IsBullishBeltHold(Indicator indicator, int trendWeight, bool isDowntrend)
        {
            if (indicator.CurrentBar < 1 || (IsBullTrend(trendWeight, isDowntrend)))
                return false;

            return indicator.Close[1] < indicator.Open[1] && indicator.Open[0] < indicator.Close[1] - 5 * indicator.TickSize && indicator.Open[0] == indicator.Low[0] && indicator.Close[0] > indicator.Open[0];
        }

        public static bool IsBullishHarami(Indicator indicator, int trendWeight, bool isDownTrend)
        {
            if (indicator.CurrentBar < 1 || (IsBullTrend(trendWeight, isDownTrend)))
                return false;

            return indicator.Close[0] > indicator.Open[0] && indicator.Close[1] < indicator.Open[1] && indicator.Low[0] >= indicator.Close[1] && indicator.High[0] <= indicator.Open[1];
        }

        public static  bool IsBullishEngulfing(Indicator indicator, int trendWeight, bool isDownTrend)
        {
            if (indicator.CurrentBar < 1 || (IsBullTrend(trendWeight, isDownTrend)))
                return false;


            return indicator.Close[1] < indicator.Open[1] && indicator.Close[0] > indicator.Open[0] && indicator.Close[0] > indicator.Open[1] && indicator.Open[0] < indicator.Close[1];
        }

        public  static bool IsEveningStar(Indicator indicator)
        {

            if (indicator.CurrentBar < 2)
                return false;

            return indicator.Close[2] > indicator.Open[2] && indicator.Close[1] > indicator.Close[2] &&
                   indicator.Open[0] < (Math.Abs((indicator.Close[1] - indicator.Open[1]) / 2) + indicator.Open[1]) && indicator.Close[0] < indicator.Open[0];
        }

        public static bool IsDownsideTasukiGap(Indicator indicator)
        {

            if (indicator.CurrentBar < 2)
                return false;

            return indicator.Close[2] < indicator.Open[2] && indicator.Close[1] < indicator.Open[1] && indicator.Close[0] > indicator.Open[0]
                   && indicator.High[1] < indicator.Low[2]
                   && indicator.Open[0] > indicator.Close[1] && indicator.Open[0] < indicator.Open[1]
                   && indicator.Close[0] > indicator.Open[1] && indicator.Close[0] < indicator.Close[2];
        }

        public static bool IsDarkCloudCover(Indicator indicater, int trendWeight, bool isUptread)
        {

            if (indicater.CurrentBar < 1 || (IsBullTrend(trendWeight, isUptread)))
                return false;

            return indicater.Open[0] > indicater.High[1] && indicater.Close[1] > indicater.Open[1] && indicater.Close[0] < indicater.Open[0] &&
                   indicater.Close[0] <= indicater.Close[1] - (indicater.Close[1] - indicater.Open[1]) / 2 && indicater.Close[0] >= indicater.Open[1];
        }

        public static bool IsBullishHaramiCross(Indicator indicater, int trendWeight, bool isDownTrend)
        {
            if (indicater.CurrentBar < 1 || (IsBullTrend(trendWeight, isDownTrend)))
                return false;

            return (indicater.High[0] <= indicater.Open[1]) && (indicater.Low[0] >= indicater.Close[1]) && indicater.Open[0] >= indicater.Close[1] && indicater.Close[0] <= indicater.Open[1] &&
                   ((indicater.Close[0] >= indicater.Open[0] && indicater.Close[0] <= indicater.Open[0] + indicater.TickSize) ||
                    (indicater.Close[0] <= indicater.Open[0] && indicater.Close[0] >= indicater.Open[0] - indicater.TickSize));
        }
    }
}