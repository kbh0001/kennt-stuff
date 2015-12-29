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


        public   bool IsBearTrend(int trendStrength, bool isUpTrend)
        {
            return trendStrength > 0 && !isUpTrend;
        }

        public   bool IsBearishBeltHold(  int trendstrenth, bool isUptrend)

        {
            if (_indicator.CurrentBar < 1 || (IsBearTrend(trendstrenth, isUptrend)))
                return false;

            return _indicator.Close[1] > _indicator.Open[1] && _indicator.Open[0] > _indicator.Close[1] + 5 * _indicator.TickSize && _indicator.Open[0] == _indicator.High[0] && _indicator.Close[0] < _indicator.Open[0];
        }

        public   bool IsBasicDoji()
        {
            return Math.Abs(_indicator.Close[0] - _indicator.Open[0]) <= (_indicator.High[0] - _indicator.Low[0]) * 0.07;
        }

        public   bool IsBearishEngulfing(  int trendStrength, bool isUptrend)
        {
            if (_indicator.CurrentBar < 1 || (IsBearTrend(trendStrength, isUptrend)))
                return false;

            return _indicator.Close[1] > _indicator.Open[1] && _indicator.Close[0] < _indicator.Open[0] && _indicator.Open[0] > _indicator.Close[1] && _indicator.Close[0] < _indicator.Open[1];
        }

        public   bool IsBearishHaramiCross(  int trendWeight, bool isUptrend)
        {

            if (_indicator.CurrentBar < 1 || (IsBearTrend(trendWeight, isUptrend)))
                return false;

            return (_indicator.High[0] <= _indicator.Close[1]) && (_indicator.Low[0] >= _indicator.Open[1]) && _indicator.Open[0] <= _indicator.Close[1] && _indicator.Close[0] >= _indicator.Open[1] &&
                   ((_indicator.Close[0] >= _indicator.Open[0] && _indicator.Close[0] <= _indicator.Open[0] + _indicator.TickSize) ||
                    (_indicator.Close[0] <= _indicator.Open[0] && _indicator.Close[0] >= _indicator.Open[0] - _indicator.TickSize));
        }

        public   bool IsBearishHarami(  int trendWeigh, bool isUptrend)
        {
            if (_indicator.CurrentBar < 1 || (IsBearTrend(trendWeigh, isUptrend)))
                return false;

            return _indicator.Close[0] < _indicator.Open[0] && _indicator.Close[1] > _indicator.Open[1] &&
                   _indicator.Low[0] >= _indicator.Open[1] && _indicator.High[0] <= _indicator.Close[1];
        }

        public   bool IsBullTrend(int trendWeight, bool isDownTrend)
        {
            return trendWeight > 0 && !isDownTrend;
        }

        public   bool IsBullishBeltHold(  int trendWeight, bool isDowntrend)
        {
            if (_indicator.CurrentBar < 1 || (IsBullTrend(trendWeight, isDowntrend)))
                return false;

            return _indicator.Close[1] < _indicator.Open[1] && _indicator.Open[0] < _indicator.Close[1] - 5 * _indicator.TickSize && _indicator.Open[0] == _indicator.Low[0] && _indicator.Close[0] > _indicator.Open[0];
        }

        public   bool IsBullishHarami(  int trendWeight, bool isDownTrend)
        {
            if (_indicator.CurrentBar < 1 || (IsBullTrend(trendWeight, isDownTrend)))
                return false;

            return _indicator.Close[0] > _indicator.Open[0] && _indicator.Close[1] < _indicator.Open[1] && _indicator.Low[0] >= _indicator.Close[1] && _indicator.High[0] <= _indicator.Open[1];
        }

        public    bool IsBullishEngulfing(  int trendWeight, bool isDownTrend)
        {
            if (_indicator.CurrentBar < 1 || (IsBullTrend(trendWeight, isDownTrend)))
                return false;


            return _indicator.Close[1] < _indicator.Open[1] && _indicator.Close[0] > _indicator.Open[0] && _indicator.Close[0] > _indicator.Open[1] && _indicator.Open[0] < _indicator.Close[1];
        }

        public    bool IsEveningStar()
        {

            if (_indicator.CurrentBar < 2)
                return false;

            return _indicator.Close[2] > _indicator.Open[2] && _indicator.Close[1] > _indicator.Close[2] &&
                   _indicator.Open[0] < (Math.Abs((_indicator.Close[1] - _indicator.Open[1]) / 2) + _indicator.Open[1]) && _indicator.Close[0] < _indicator.Open[0];
        }

        public   bool IsDownsideTasukiGap()
        {

            if (_indicator.CurrentBar < 2)
                return false;

            return _indicator.Close[2] < _indicator.Open[2] && _indicator.Close[1] < _indicator.Open[1] && _indicator.Close[0] > _indicator.Open[0]
                   && _indicator.High[1] < _indicator.Low[2]
                   && _indicator.Open[0] > _indicator.Close[1] && _indicator.Open[0] < _indicator.Open[1]
                   && _indicator.Close[0] > _indicator.Open[1] && _indicator.Close[0] < _indicator.Close[2];
        }

        public   bool IsDarkCloudCover(int trendWeight, bool isUptread)
        {

            if (_indicator.CurrentBar < 1 || (IsBullTrend(trendWeight, isUptread)))
                return false;

            return _indicator.Open[0] > _indicator.High[1] && _indicator.Close[1] > _indicator.Open[1] && _indicator.Close[0] < _indicator.Open[0] &&
                   _indicator.Close[0] <= _indicator.Close[1] - (_indicator.Close[1] - _indicator.Open[1]) / 2 && _indicator.Close[0] >= _indicator.Open[1];
        }

        public   bool IsBullishHaramiCross( int trendWeight, bool isDownTrend)
        {
            if (_indicator.CurrentBar < 1 || (IsBullTrend(trendWeight, isDownTrend)))
                return false;

            return (_indicator.High[0] <= _indicator.Open[1]) && (_indicator.Low[0] >= _indicator.Close[1]) && _indicator.Open[0] >= _indicator.Close[1] && _indicator.Close[0] <= _indicator.Open[1] &&
                   ((_indicator.Close[0] >= _indicator.Open[0] && _indicator.Close[0] <= _indicator.Open[0] + _indicator.TickSize) ||
                    (_indicator.Close[0] <= _indicator.Open[0] && _indicator.Close[0] >= _indicator.Open[0] - _indicator.TickSize));
        }

        public   bool IsInvertedHammer(  int trendWeight, bool isUptrend)
        {

            if (trendWeight > 0)
            {
                if (!isUptrend || _indicator.MAX(_indicator.High, trendWeight)[0] != _indicator.High[0])
                    return false;
            }

            return _indicator.High[0] > _indicator.Open[0] + 5 * _indicator.TickSize && Math.Abs(_indicator.Open[0] - _indicator.Close[0]) < (0.10 * (_indicator.High[0] - _indicator.Low[0])) &&
                   (_indicator.Close[0] - _indicator.Low[0]) < (0.25 * (_indicator.High[0] - _indicator.Low[0]));
        }

        public   bool IsHangingMan(  int trendWeight, bool isUptrend)
        {
            if (trendWeight > 0)
            {
                if (!isUptrend || _indicator.MAX(_indicator.High, trendWeight)[0] != _indicator.High[0])
                    return false;
            }


            return _indicator.Low[0] < _indicator.Open[0] - 5 * _indicator.TickSize && Math.Abs(_indicator.Open[0] - _indicator.Close[0]) < (0.10 * (_indicator.High[0] - _indicator.Low[0])) &&
                   (_indicator.High[0] - _indicator.Close[0]) < (0.25 * (_indicator.High[0] - _indicator.Low[0]));
        }

        public   bool IsHammer(  int trendWeight, bool isDownTrend)
        {
            if (trendWeight > 0)
            {
                if (!isDownTrend || _indicator.MIN(_indicator.Low, trendWeight)[0] != _indicator.Low[0])
                    return false;
            }


            return _indicator.Low[0] < _indicator.Open[0] - 5 * _indicator.TickSize && Math.Abs(_indicator.Open[0] - _indicator.Close[0]) < (0.10 * (_indicator.High[0] - _indicator.Low[0])) &&
                   (_indicator.High[0] - _indicator.Close[0]) < (0.25 * (_indicator.High[0] - _indicator.Low[0]));
        }

        public   bool IsFallingThree()
        {

            if (_indicator.CurrentBar < 5)
                return false;

            return _indicator.Close[4] < _indicator.Open[4] && _indicator.Close[0] < _indicator.Open[0] && _indicator.Close[0] < _indicator.Low[4]
                   && _indicator.High[3] < _indicator.High[4] && _indicator.Low[3] > _indicator.Low[4]
                   && _indicator.High[2] < _indicator.High[4] && _indicator.Low[2] > _indicator.Low[4]
                   && _indicator.High[1] < _indicator.High[4] && _indicator.Low[1] > _indicator.Low[4];
        }

        public   bool IsUpsideTasukiGap()
        {

            if (_indicator.CurrentBar < 2)
                return false;

            return _indicator.Close[2] > _indicator.Open[2] && _indicator.Close[1] > _indicator.Open[1] && _indicator.Close[0] < _indicator.Open[0]
                   && _indicator.Low[1] > _indicator.High[2]
                   && _indicator.Open[0] < _indicator.Close[1] && _indicator.Open[0] > _indicator.Open[1]
                   && _indicator.Close[0] < _indicator.Open[1] && _indicator.Close[0] > _indicator.Close[2];
        }

        public   bool IsUpsideGapTwoCrows(  int trendWeight, bool isUpTrend)
        {

            if (_indicator.CurrentBar < 2 || (trendWeight > 0 && !isUpTrend))
                return false;

            return _indicator.Close[2] > _indicator.Open[2] && _indicator.Close[1] < _indicator.Open[1] && _indicator.Close[0] < _indicator.Open[0]
                   && _indicator.Low[1] > _indicator.High[2]
                   && _indicator.Close[0] > _indicator.High[2]
                   && _indicator.Close[0] < _indicator.Close[1] && _indicator.Open[0] > _indicator.Open[1];
        }

        public   bool IsThreeWhiteSoldiers(  int trendWeight, bool isDownTrend)
        {

            if (_indicator.CurrentBar < 2 || (IsBullTrend(trendWeight, isDownTrend)))
                return false;

            return _indicator.Value[1] == 0 && _indicator.Value[2] == 0
                   && _indicator.Close[0] > _indicator.Open[0] && _indicator.Close[1] > _indicator.Open[1] && _indicator.Close[2] > _indicator.Open[2]
                   && _indicator.Close[0] > _indicator.Close[1] && _indicator.Close[1] > _indicator.Close[2]
                   && _indicator.Open[0] < _indicator.Close[1] && _indicator.Open[0] > _indicator.Open[1]
                   && _indicator.Open[1] < _indicator.Close[2] && _indicator.Open[1] > _indicator.Open[2];
        }

        public   bool IsThreeBlackCrows(  int trendWeight, bool isUpTrend)
        {
            if (_indicator.CurrentBar < 2 || (trendWeight > 0 && !isUpTrend))
                return false;


            return _indicator.Value[1] == 0 && _indicator.Value[2] == 0
                   && _indicator.Close[0] < _indicator.Open[0] && _indicator.Close[1] < _indicator.Open[1] && _indicator.Close[2] < _indicator.Open[2]
                   && _indicator.Close[0] < _indicator.Close[1] && _indicator.Close[1] < _indicator.Close[2]
                   && _indicator.Open[0] < _indicator.Open[1] && _indicator.Open[0] > _indicator.Close[1]
                   && _indicator.Open[1] < _indicator.Open[2] && _indicator.Open[1] > _indicator.Close[2];
        }

        public    bool IsStickSandwich()
        {
            if (_indicator.CurrentBar < 2)
                return false;

            return _indicator.Close[2] == _indicator.Close[0] && _indicator.Close[2] < _indicator.Open[2] && _indicator.Close[1] > _indicator.Open[1] && _indicator.Close[0] < _indicator.Open[0];
        }

        public   bool IsShootingStar(  int trendWeight, bool isUpTrend)
        {

            if (_indicator.CurrentBar < 1 || (trendWeight > 0 && !isUpTrend))
                return false;


            return _indicator.High[0] > _indicator.Open[0] && (_indicator.High[0] - _indicator.Open[0]) >= 2 * (_indicator.Open[0] - _indicator.Close[0]) && _indicator.Close[0] < _indicator.Open[0] &&
                   (_indicator.Close[0] - _indicator.Low[0]) <= 2 * _indicator.TickSize;
        }

        public   bool IsRisingThree()
        {

            if (_indicator.CurrentBar < 5)
                return false;

            return _indicator.Close[4] > _indicator.Open[4] && _indicator.Close[0] > _indicator.Open[0] && _indicator.Close[0] > _indicator.High[4]
                   && _indicator.High[3] < _indicator.High[4] && _indicator.Low[3] > _indicator.Low[4]
                   && _indicator.High[2] < _indicator.High[4] && _indicator.Low[2] > _indicator.Low[4]
                   && _indicator.High[1] < _indicator.High[4] && _indicator.Low[1] > _indicator.Low[4];
        }

        public   bool IsPiercingLine(  int trendWeight, bool isDownTrend)
        {
            if (_indicator.CurrentBar < 1 || (IsBullTrend(trendWeight, isDownTrend)))
                return false;
            return _indicator.Open[0] < _indicator.Low[1] && _indicator.Close[1] < _indicator.Open[1] &&
                   _indicator.Close[0] > _indicator.Open[0] &&
                   _indicator.Close[0] >= _indicator.Close[1] + (_indicator.Open[1] - _indicator.Close[1])/2 &&
                   _indicator.Close[0] <= _indicator.Open[1];
        }

        public   bool IsMorningStar()
        {
            if (_indicator.CurrentBar < 2)
                return false;

            return _indicator.Close[2] < _indicator.Open[2] && _indicator.Close[1] < _indicator.Close[2] &&
                   _indicator.Open[0] > (Math.Abs((_indicator.Close[1] - _indicator.Open[1])/2) + _indicator.Open[1]) &&
                   _indicator.Close[0] > _indicator.Open[0];
        }
    }
}