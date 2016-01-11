using System;
using System.Linq;
using NinjaTrader.Data;
using NinjaTrader.Indicator;

namespace KenNinja
{
    public class KenCandleStickDeterminer
    {
        private readonly IndicatorBase _indicator;
        private readonly Action<string> _loggerDelegate;
        public int _trendWeight;

        private bool isDownTrend;
        private bool isUpTrend;
        private Func<int, double> _stocFunc;

        public KenCandleStickDeterminer(IndicatorBase indicator, int trendWeight, Func<int, double> stocFunc,  Action<string> logger = null)
        {
            _indicator = indicator;
            _trendWeight = trendWeight;
            _loggerDelegate = logger;
            _stocFunc = stocFunc;
            CalculateTrendLines();
        }


        public bool IsBearTrend
        {
            get { return _trendWeight > 0 && !isUpTrend; }
        }

        public bool IsBearishBeltHold
        {
            get
            {
                if (_indicator.CurrentBar < 1 || (IsBearTrend))
                    return false;

                return _indicator.Close[1] > _indicator.Open[1] &&
                       _indicator.Open[0] > _indicator.Close[1] + 5*_indicator.TickSize &&
                       _indicator.Open[0] == _indicator.High[0] && _indicator.Close[0] < _indicator.Open[0];
            }
        }

        public bool IsDoji
        {
            get { return DetermineIsDojiFor(); }
        }

        public bool IsBullishDoji
        {
            get
            {
                if (_indicator.CurrentBar < 4)

                if (_stocFunc(0) < 25)
                    return false;



                return (DetermineIsDojiFor() && IsBearCandle(1) && IsBearCandle(2));
            }
        }


        public bool IsBearishDoji
        {
            get
            {
                if (_indicator.CurrentBar < 1 || WasBearTrend(1))
                    return false;

                if (_stocFunc(1) > 75)
                    return false;

                return (_indicator.CurrentBar >= 2 && WasBullTrend(1) && DetermineIsDojiFor());
            }
        }

        public bool IsBearishEngulfing
        {
            get
            {
                if (_indicator.CurrentBar < 1 || (IsBearTrend))
                    return false;

                return _indicator.Close[1] > _indicator.Open[1] && _indicator.Close[0] < _indicator.Open[0] &&
                       _indicator.Open[0] > _indicator.Close[1] && _indicator.Close[0] < _indicator.Open[1];
            }
        }

        public bool IsBearishHaramiCross
        {
            get
            {
                if (_indicator.CurrentBar < 1 || (IsBearTrend))
                    return false;

                return (_indicator.High[0] <= _indicator.Close[1]) && (_indicator.Low[0] >= _indicator.Open[1]) &&
                       _indicator.Open[0] <= _indicator.Close[1] && _indicator.Close[0] >= _indicator.Open[1] &&
                       ((_indicator.Close[0] >= _indicator.Open[0] &&
                         _indicator.Close[0] <= _indicator.Open[0] + _indicator.TickSize) ||
                        (_indicator.Close[0] <= _indicator.Open[0] &&
                         _indicator.Close[0] >= _indicator.Open[0] - _indicator.TickSize));
            }
        }

        public bool IsBearishHarami
        {
            get
            {
                if (_indicator.CurrentBar < 1 || (IsBearTrend))
                    return false;

                return _indicator.Close[0] < _indicator.Open[0] && _indicator.Close[1] > _indicator.Open[1] &&
                       _indicator.Low[0] >= _indicator.Open[1] && _indicator.High[0] <= _indicator.Close[1];
            }
        }

        public bool IsBullTrend
        {
            get { return _trendWeight > 0 && !isDownTrend; }
        }

        public bool IsBullishBeltHold
        {
            get
            {
                if (_indicator.CurrentBar < 1 || (IsBullTrend))
                    return false;

                return _indicator.Close[1] < _indicator.Open[1] &&
                       _indicator.Open[0] < _indicator.Close[1] - 5*_indicator.TickSize &&
                       _indicator.Open[0] == _indicator.Low[0] && _indicator.Close[0] > _indicator.Open[0];
            }
        }

        //asdf

        public bool IsBullishHarami
        {
            get
            {
                if (_indicator.CurrentBar < 2 || (IsBullTrend))
                    return false;

                return IsBullCandle() && IsBearCandle(1) &&
                       _indicator.Low[0] > _indicator.Close[1] && _indicator.High[0] < _indicator.Open[1];
            }
        }

        public bool IsBullishEngulfing
        {
            get
            {
                if (_indicator.CurrentBar < 2 || (IsBullTrend))
                    return false;


                return IsBearCandle(1) && IsBullCandle(0) &&
                       _indicator.Close[0] > _indicator.Open[1] && _indicator.Open[0] < _indicator.Close[1];
            }
        }

        public bool IsEveningStar
        {
            get
            {
                if (_indicator.CurrentBar < 2)
                    return false;

                return _indicator.Close[2] > _indicator.Open[2] && _indicator.Close[1] > _indicator.Close[2] &&
                       _indicator.Open[0] <
                       (Math.Abs((_indicator.Close[1] - _indicator.Open[1])/2) + _indicator.Open[1]) &&
                       _indicator.Close[0] < _indicator.Open[0];
            }
        }

        public bool IsBearishDownsideTasukiGap
        {
            get
            {

                if (IsBullTrend)
                    return false;
                
                if (_indicator.CurrentBar < 2)
                    return false;

                return _indicator.Close[2] < _indicator.Open[2] && _indicator.Close[1] < _indicator.Open[1] &&
                       _indicator.Close[0] > _indicator.Open[0]
                       && _indicator.High[1] < _indicator.Low[2]
                       && _indicator.Open[0] > _indicator.Close[1] && _indicator.Open[0] < _indicator.Open[1]
                       && _indicator.Close[0] > _indicator.Open[1] && _indicator.Close[0] < _indicator.Close[2];
            }
        }

        public bool IsDarkCloudCover
        {
            get
            {
                if (_indicator.CurrentBar < 1 || (IsBullTrend))
                    return false;

                return _indicator.Open[0] > _indicator.High[1] && _indicator.Close[1] > _indicator.Open[1] &&
                       _indicator.Close[0] < _indicator.Open[0] &&
                       _indicator.Close[0] <= _indicator.Close[1] - (_indicator.Close[1] - _indicator.Open[1])/2 &&
                       _indicator.Close[0] >= _indicator.Open[1];
            }
        }

        public bool IsBullishHaramiCross
        {
            get
            {
                if (_indicator.CurrentBar < 1 || (IsBullTrend))
                    return false;

                return (_indicator.High[0] <= _indicator.Open[1]) && (_indicator.Low[0] >= _indicator.Close[1]) &&
                       _indicator.Open[0] >= _indicator.Close[1] && _indicator.Close[0] <= _indicator.Open[1] &&
                       ((_indicator.Close[0] >= _indicator.Open[0] &&
                         _indicator.Close[0] <= _indicator.Open[0] + _indicator.TickSize) ||
                        (_indicator.Close[0] <= _indicator.Open[0] &&
                         _indicator.Close[0] >= _indicator.Open[0] - _indicator.TickSize));
            }
        }

        public bool IsInvertedHammer
        {
            get
            {
                if (_trendWeight > 0)
                {
                    if (_indicator.CurrentBar > _trendWeight)
                    {
                        var max = Enumerable.Range(0, _trendWeight).Select(x => _indicator.High[x]).Max();
                        if (!isUpTrend || max != _indicator.High[0])
                            return false;
                    }
                    else
                    {
                        return false;
                    }
                }

                return _indicator.High[0] > _indicator.Open[0] + 5*_indicator.TickSize &&
                       Math.Abs(_indicator.Open[0] - _indicator.Close[0]) <
                       (0.10*(_indicator.High[0] - _indicator.Low[0])) &&
                       (_indicator.Close[0] - _indicator.Low[0]) < (0.25*(_indicator.High[0] - _indicator.Low[0]));
            }
        }

        public bool IsHangingMan
        {
            get
            {
                if (_trendWeight > 0)
                {
                    if (_indicator.CurrentBar > _trendWeight)
                    {
                        var max = Enumerable.Range(0, _trendWeight).Select(x => _indicator.High[x]).Max();
                        if (!isUpTrend || max != _indicator.High[0])
                            return false;
                    }
                    else
                        return false;
                }


                return _indicator.Low[0] < _indicator.Open[0] - 5*_indicator.TickSize &&
                       Math.Abs(_indicator.Open[0] - _indicator.Close[0]) <
                       (0.10*(_indicator.High[0] - _indicator.Low[0])) &&
                       (_indicator.High[0] - _indicator.Close[0]) < (0.25*(_indicator.High[0] - _indicator.Low[0]));
            }
        }

        public bool IsHammer
        {
            get
            {
                if (_trendWeight > 0)
                {
                    if (_indicator.CurrentBar > _trendWeight)
                    {
                        var min = Enumerable.Range(0, _trendWeight).Select(x => _indicator.Low[x]).Min();
                        if (!isDownTrend || min != _indicator.Low[0])
                            return false;
                    }
                    else
                        return false;
                }


                return _indicator.Low[0] < _indicator.Open[0] - 5*_indicator.TickSize &&
                       Math.Abs(_indicator.Open[0] - _indicator.Close[0]) <
                       (0.10*(_indicator.High[0] - _indicator.Low[0])) &&
                       (_indicator.High[0] - _indicator.Close[0]) < (0.25*(_indicator.High[0] - _indicator.Low[0]));
            }
        }

        public bool IsFallingThreeMethods
        {
            get
            {
                if (_indicator.CurrentBar < 5)
                    return false;

                return _indicator.Close[4] < _indicator.Open[4] && _indicator.Close[0] < _indicator.Open[0] &&
                       _indicator.Close[0] < _indicator.Low[4]
                       && _indicator.High[3] < _indicator.High[4] && _indicator.Low[3] > _indicator.Low[4]
                       && _indicator.High[2] < _indicator.High[4] && _indicator.Low[2] > _indicator.Low[4]
                       && _indicator.High[1] < _indicator.High[4] && _indicator.Low[1] > _indicator.Low[4];
            }
        }

        public bool IsBullishUpsideTasukiGap
        {
            get
            {
                if (IsBearTrend)
                    return false;

                if (_indicator.CurrentBar < 2)
                    return false;

                return _indicator.Close[2] > _indicator.Open[2] && _indicator.Close[1] > _indicator.Open[1] &&
                       _indicator.Close[0] < _indicator.Open[0]
                       && _indicator.Low[1] > _indicator.High[2]
                       && _indicator.Open[0] < _indicator.Close[1] && _indicator.Open[0] > _indicator.Open[1]
                       && _indicator.Close[0] < _indicator.Open[1] && _indicator.Close[0] > _indicator.Close[2];
            }
        }

        public bool IsUpsideGapTwoCrows
        {
            get
            {
                if (_indicator.CurrentBar < 2 || (_trendWeight > 0 && !isUpTrend))
                    return false;

                return _indicator.Close[2] > _indicator.Open[2] && _indicator.Close[1] < _indicator.Open[1] &&
                       _indicator.Close[0] < _indicator.Open[0]
                       && _indicator.Low[1] > _indicator.High[2]
                       && _indicator.Close[0] > _indicator.High[2]
                       && _indicator.Close[0] < _indicator.Close[1] && _indicator.Open[0] > _indicator.Open[1];
            }
        }

        public bool IsThreeWhiteSoldiers
        {
            get
            {
                if (_indicator.CurrentBar < 2 || (IsBullTrend))
                    return false;

                return _indicator.Value[1] == 0 && _indicator.Value[2] == 0
                       && _indicator.Close[0] > _indicator.Open[0] && _indicator.Close[1] > _indicator.Open[1] &&
                       _indicator.Close[2] > _indicator.Open[2]
                       && _indicator.Close[0] > _indicator.Close[1] && _indicator.Close[1] > _indicator.Close[2]
                       && _indicator.Open[0] < _indicator.Close[1] && _indicator.Open[0] > _indicator.Open[1]
                       && _indicator.Open[1] < _indicator.Close[2] && _indicator.Open[1] > _indicator.Open[2];
            }
        }

        public bool IsThreeBlackCrows
        {
            get
            {
                if (_indicator.CurrentBar < 2 || (_trendWeight > 0 && !isUpTrend))
                    return false;


                return _indicator.Value[1] == 0 && _indicator.Value[2] == 0
                       && _indicator.Close[0] < _indicator.Open[0] && _indicator.Close[1] < _indicator.Open[1] &&
                       _indicator.Close[2] < _indicator.Open[2]
                       && _indicator.Close[0] < _indicator.Close[1] && _indicator.Close[1] < _indicator.Close[2]
                       && _indicator.Open[0] < _indicator.Open[1] && _indicator.Open[0] > _indicator.Close[1]
                       && _indicator.Open[1] < _indicator.Open[2] && _indicator.Open[1] > _indicator.Close[2];
            }
        }

        public bool IsBearishStickSandwich
        {
            get
            {
               if (_indicator.CurrentBar < 2 || (_trendWeight > 0 && isUpTrend))
                  return false;

                var barQuartile = Math.Abs(_indicator.Close[2] - _indicator.Open[2])*.25;

                return _indicator.Open[2] > _indicator.Close[2] //down bar 2
                       && _indicator.Open[1] < _indicator.Close[1] // up bar 1
                       && _indicator.Open[0] > _indicator.Close[0] //down bar 0
                       && _indicator.Open[1] > _indicator.Open[2] //trades higher than bar 2
                       && _indicator.Open[0] >= _indicator.Open[2] - barQuartile //bar 0 opens close to bar 2 
                       && _indicator.Open[0] <= _indicator.Open[2] + barQuartile;
            }
        }


        public bool IsBullishStickSandwich
        {
            get
            {
                if (_indicator.CurrentBar < 2 || (_trendWeight > 0 && isDownTrend))
                    return false;


                var barQuartile = Math.Abs(_indicator.Close[2] - _indicator.Open[2])*.25;

                return _indicator.Open[2] < _indicator.Close[2] //Up bar 2
                       && _indicator.Open[1] > _indicator.Close[1] // down bar 1
                       && _indicator.Open[0] < _indicator.Close[0] //Up bar 0
                       && _indicator.Open[1] < _indicator.Open[2] //trades lower than bar 2
                       && _indicator.Open[0] >= _indicator.Open[2] - barQuartile //bar 0 opens close to bar 2 
                       && _indicator.Open[0] <= _indicator.Open[2] + barQuartile;
            }
        }


        public bool IsShootingStar
        {
            get
            {
                if (_indicator.CurrentBar < 1 || (_trendWeight > 0 && !isUpTrend))
                    return false;

                if (_stocFunc(0) < 70)
                    return false;



                var seemsToBeStar =  _indicator.High[0] > _indicator.Open[0] &&
                       (_indicator.High[0] - _indicator.Open[0]) >= 2*(_indicator.Open[0] - _indicator.Close[0]) &&
                       _indicator.Close[0] < _indicator.Open[0] &&
                       (_indicator.Close[0] - _indicator.Low[0]) <= 0;


                return seemsToBeStar;

            }
        }

        public bool IsRisingThree
        {
            get
            {
                if (_indicator.CurrentBar < 5)
                    return false;

                return _indicator.Close[4] > _indicator.Open[4] && _indicator.Close[0] > _indicator.Open[0] &&
                       _indicator.Close[0] > _indicator.High[4]
                       && _indicator.High[3] < _indicator.High[4] && _indicator.Low[3] > _indicator.Low[4]
                       && _indicator.High[2] < _indicator.High[4] && _indicator.Low[2] > _indicator.Low[4]
                       && _indicator.High[1] < _indicator.High[4] && _indicator.Low[1] > _indicator.Low[4];
            }
        }

        public bool IsPiercingLine
        {
            get
            {
                if (_indicator.CurrentBar < 1 || (IsBullTrend))
                    return false;
                return _indicator.Open[0] < _indicator.Low[1] && _indicator.Close[1] < _indicator.Open[1] &&
                       _indicator.Close[0] > _indicator.Open[0] &&
                       _indicator.Close[0] >= _indicator.Close[1] + (_indicator.Open[1] - _indicator.Close[1])/2 &&
                       _indicator.Close[0] <= _indicator.Open[1];
            }
        }

        public bool IsMorningStar
        {
            get
            {
                if (_indicator.CurrentBar < 3)
                    return false;

                return IsBearCandle(2) && _indicator.Close[1] < _indicator.Close[2] &&
                       _indicator.Open[0] >
                       (Math.Abs((_indicator.Close[1] - _indicator.Open[1])/2) + _indicator.Open[1]) &&
                       IsBullCandle();
            }
        }


        public bool IsBottomBounce
        {
            get
            {
                if (!IsBearTrend)
                    return false;

                if (_indicator.CurrentBar < 4)
                    return false;

                if (!(IsBearCandle(3) && IsBearCandle(2) && IsBullCandle(1) && IsBullCandle()))
                    return false;

                if (!HasDepth(3) || !HasDepth(2) || !HasDepth(1) || !HasDepth())
                    return false;


                if (this._stocFunc(2) > 19)
                    return false;



                var bounceThreshold = (_indicator.Open[2] + _indicator.Close[2])/2;

                return _indicator.Close[1] > bounceThreshold && _indicator.Close[2] == _indicator.Open[1];




            }
        }

        private double Sma(IDataSeries data, int periods, int backPeriods = 0)
        {
            return Enumerable.Range(backPeriods, periods).Select(z => data[z]).Average();
        }

        private void Log(string logMessage)
        {
            if (_loggerDelegate != null)
            {
                _loggerDelegate(logMessage);
            }
        }

        private bool HasDepth(int barsAgo = 0)
        {
            return Math.Abs(_indicator.Close[0] - _indicator.Open[0]) > 0;
        }

        private bool IsBearCandle(int barsAgo = 0)
        {
            return _indicator.Close[barsAgo] < _indicator.Open[barsAgo];
        }

        private bool IsBullCandle(int barsAgo = 0)
        {
            return _indicator.Close[barsAgo] > _indicator.Open[barsAgo];
        }


        private bool WasBullTrend(int backPeriods)
        {
            if (_trendWeight <= 0)
                return false;

            if (_indicator.CurrentBar <= (_trendWeight*2) + backPeriods)
                return false;
            var sma = Sma(_indicator.Close, _trendWeight, backPeriods);
            var smalong = Sma(_indicator.Close, _trendWeight*2, backPeriods);

            return sma > smalong;
        }


        private bool WasBearTrend(int backPeriods)
        {
            if (_trendWeight <= 0)
                return false;

            if (_indicator.CurrentBar <= (_trendWeight*2) + backPeriods)
                return false;
            var sma = Sma(_indicator.Close, _trendWeight, backPeriods);
            var smalong = Sma(_indicator.Close, _trendWeight*2, backPeriods);

            return sma < smalong;
        }

        private void CalculateTrendLines()
        {
            isUpTrend = false;
            isDownTrend = false;

            if (_trendWeight <= 0)
                return;


            if (_indicator.CurrentBar <= _trendWeight*2)
                return;


            var sma = Sma(_indicator.Close, _trendWeight);
            var smalong = Sma(_indicator.Close, _trendWeight*2);

            var smaBear = Sma(_indicator.Close, _trendWeight/2);
            var smaBearlong = Sma(_indicator.Close, _trendWeight);


            if (smaBear < smaBearlong)
            {
                isUpTrend = false;
                isDownTrend = true;
            }

            if (sma > smalong)
            {
                isUpTrend = true;
                isDownTrend = false;
            }
        }

        private bool DetermineIsDojiFor(int backPeriods = 0)
        {
            var lookAt = 0;
            lookAt += backPeriods;
            return Math.Abs(_indicator.Close[lookAt] - _indicator.Open[lookAt]) <=
                   (_indicator.High[lookAt] - _indicator.Low[lookAt])*0.07;
        }
    }
}