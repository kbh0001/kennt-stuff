using System;

namespace NinjaTrader.Indicator
{
    public class KenCandleStickDeterminer
    {

        private Indicator _indicator;

        public  KenCandleStickDeterminer(Indicator indicator)
        {
            _indicator = indicator;
        }


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

        public static bool IsInvertedHammer(Indicator indicator, int trendWeight, bool isUptrend)
        {

            if (trendWeight > 0)
            {
                if (!isUptrend || indicator.MAX(indicator.High, trendWeight)[0] != indicator.High[0])
                    return false;
            }

            return indicator.High[0] > indicator.Open[0] + 5 * indicator.TickSize && Math.Abs(indicator.Open[0] - indicator.Close[0]) < (0.10 * (indicator.High[0] - indicator.Low[0])) &&
                   (indicator.Close[0] - indicator.Low[0]) < (0.25 * (indicator.High[0] - indicator.Low[0]));
        }

        public static bool IsHangingMan(Indicator indicator, int trendWeight, bool isUptrend)
        {
            if (trendWeight > 0)
            {
                if (!isUptrend || indicator.MAX(indicator.High, trendWeight)[0] != indicator.High[0])
                    return false;
            }


            return indicator.Low[0] < indicator.Open[0] - 5 * indicator.TickSize && Math.Abs(indicator.Open[0] - indicator.Close[0]) < (0.10 * (indicator.High[0] - indicator.Low[0])) &&
                   (indicator.High[0] - indicator.Close[0]) < (0.25 * (indicator.High[0] - indicator.Low[0]));
        }

        public static bool IsHammer(Indicator indicator, int trendWeight, bool isDownTrend)
        {
            if (trendWeight > 0)
            {
                if (!isDownTrend || indicator.MIN(indicator.Low, trendWeight)[0] != indicator.Low[0])
                    return false;
            }


            return indicator.Low[0] < indicator.Open[0] - 5 * indicator.TickSize && Math.Abs(indicator.Open[0] - indicator.Close[0]) < (0.10 * (indicator.High[0] - indicator.Low[0])) &&
                   (indicator.High[0] - indicator.Close[0]) < (0.25 * (indicator.High[0] - indicator.Low[0]));
        }

        public static bool IsFallingThree(Indicator indicator)
        {

            if (indicator.CurrentBar < 5)
                return false;

            return indicator.Close[4] < indicator.Open[4] && indicator.Close[0] < indicator.Open[0] && indicator.Close[0] < indicator.Low[4]
                   && indicator.High[3] < indicator.High[4] && indicator.Low[3] > indicator.Low[4]
                   && indicator.High[2] < indicator.High[4] && indicator.Low[2] > indicator.Low[4]
                   && indicator.High[1] < indicator.High[4] && indicator.Low[1] > indicator.Low[4];
        }

        public static bool IsUpsideTasukiGap(Indicator indicator)
        {

            if (indicator.CurrentBar < 2)
                return false;

            return indicator.Close[2] > indicator.Open[2] && indicator.Close[1] > indicator.Open[1] && indicator.Close[0] < indicator.Open[0]
                   && indicator.Low[1] > indicator.High[2]
                   && indicator.Open[0] < indicator.Close[1] && indicator.Open[0] > indicator.Open[1]
                   && indicator.Close[0] < indicator.Open[1] && indicator.Close[0] > indicator.Close[2];
        }

        public static bool IsUpsideGapTwoCrows(Indicator indicator, int trendWeight, bool isUpTrend)
        {

            if (indicator.CurrentBar < 2 || (trendWeight > 0 && !isUpTrend))
                return false;

            return indicator.Close[2] > indicator.Open[2] && indicator.Close[1] < indicator.Open[1] && indicator.Close[0] < indicator.Open[0]
                   && indicator.Low[1] > indicator.High[2]
                   && indicator.Close[0] > indicator.High[2]
                   && indicator.Close[0] < indicator.Close[1] && indicator.Open[0] > indicator.Open[1];
        }

        public static bool IsThreeWhiteSoldiers(Indicator indicator, int trendWeight, bool isDownTrend)
        {

            if (indicator.CurrentBar < 2 || (IsBullTrend(trendWeight, isDownTrend)))
                return false;

            return indicator.Value[1] == 0 && indicator.Value[2] == 0
                   && indicator.Close[0] > indicator.Open[0] && indicator.Close[1] > indicator.Open[1] && indicator.Close[2] > indicator.Open[2]
                   && indicator.Close[0] > indicator.Close[1] && indicator.Close[1] > indicator.Close[2]
                   && indicator.Open[0] < indicator.Close[1] && indicator.Open[0] > indicator.Open[1]
                   && indicator.Open[1] < indicator.Close[2] && indicator.Open[1] > indicator.Open[2];
        }

        public static bool IsThreeBlackCrows(Indicator indicator, int trendWeight, bool isUpTrend)
        {
            if (indicator.CurrentBar < 2 || (trendWeight > 0 && !isUpTrend))
                return false;


            return indicator.Value[1] == 0 && indicator.Value[2] == 0
                   && indicator.Close[0] < indicator.Open[0] && indicator.Close[1] < indicator.Open[1] && indicator.Close[2] < indicator.Open[2]
                   && indicator.Close[0] < indicator.Close[1] && indicator.Close[1] < indicator.Close[2]
                   && indicator.Open[0] < indicator.Open[1] && indicator.Open[0] > indicator.Close[1]
                   && indicator.Open[1] < indicator.Open[2] && indicator.Open[1] > indicator.Close[2];
        }

        public  static bool IsStickSandwich(Indicator indicator)
        {
            if (indicator.CurrentBar < 2)
                return false;

            return indicator.Close[2] == indicator.Close[0] && indicator.Close[2] < indicator.Open[2] && indicator.Close[1] > indicator.Open[1] && indicator.Close[0] < indicator.Open[0];
        }

        public static bool IsShootingStar(Indicator indicator, int trendWeight, bool isUpTrend)
        {

            if (indicator.CurrentBar < 1 || (trendWeight > 0 && !isUpTrend))
                return false;


            return indicator.High[0] > indicator.Open[0] && (indicator.High[0] - indicator.Open[0]) >= 2 * (indicator.Open[0] - indicator.Close[0]) && indicator.Close[0] < indicator.Open[0] &&
                   (indicator.Close[0] - indicator.Low[0]) <= 2 * indicator.TickSize;
        }

        public static bool IsRisingThree(Indicator indicator)
        {

            if (indicator.CurrentBar < 5)
                return false;

            return indicator.Close[4] > indicator.Open[4] && indicator.Close[0] > indicator.Open[0] && indicator.Close[0] > indicator.High[4]
                   && indicator.High[3] < indicator.High[4] && indicator.Low[3] > indicator.Low[4]
                   && indicator.High[2] < indicator.High[4] && indicator.Low[2] > indicator.Low[4]
                   && indicator.High[1] < indicator.High[4] && indicator.Low[1] > indicator.Low[4];
        }

        public static bool IsPiercingLine(Indicator indicator, int trendWeight, bool isDownTrend)
        {
            if (indicator.CurrentBar < 1 || (IsBullTrend(trendWeight, isDownTrend)))
                return false;
            return indicator.Open[0] < indicator.Low[1] && indicator.Close[1] < indicator.Open[1] &&
                   indicator.Close[0] > indicator.Open[0] &&
                   indicator.Close[0] >= indicator.Close[1] + (indicator.Open[1] - indicator.Close[1])/2 &&
                   indicator.Close[0] <= indicator.Open[1];
        }

        public static bool IsMorningStar(Indicator indicator)
        {
            if (indicator.CurrentBar < 2)
                return false;

            return indicator.Close[2] < indicator.Open[2] && indicator.Close[1] < indicator.Close[2] &&
                   indicator.Open[0] > (Math.Abs((indicator.Close[1] - indicator.Open[1])/2) + indicator.Open[1]) &&
                   indicator.Close[0] > indicator.Open[0];
        }
    }
}