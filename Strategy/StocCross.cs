// This namespace holds all strategies and is required. Do not change it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using KenNinja;
using NinjaTrader.Cbi;
using NinjaTrader.Indicator;

namespace NinjaTrader.Custom.Strategy



{
    /// <summary>
    /// Get Some Data
    /// </summary>
    /// 
    /// 
    /// 
    //
    [Description("Get Some Data")]
    public class StocCross : NinjaTrader.Strategy.Strategy
    {
        // User defined variables (add any user defined variables below)

        private const int TrendStrength = 4;
        private static readonly List<Kp> KpsToUse;
        private static int tradeId = 0;
        private double _strikeWidth;
        private MoveActiveOrderTracker _activeOrderTracker;


        //Configure the allowed patterns that are significant order by performance.
        static StocCross()
        {
        }

        


        protected override void Initialize()
        {
            var instrument = Instrument.ToString().ToUpper().Replace("DEFAULT", "").Replace(" ", "");
            _strikeWidth = MoveBinaryStrikeWidthProvider.GetBinaryStrikeWidthFor(instrument);
            _activeOrderTracker = new MoveActiveOrderTracker();
            Log(string.Format("Starting for KenCandleStickStrategy {0}", Instrument), LogLevel.Information);
            CalculateOnBarClose = true; //only on bar close
        }


        protected override void OnBarUpdate()
        {
            try
            {

                var barTime = DateTime.Parse(Time.ToString());
                _activeOrderTracker.HandleCurrentOrders(barTime, Open[0], Close[0], High[0], Low[0]);


                Print("");

                Print(string.Format("{0} of {1} bulls successful({2})", _activeOrderTracker.WinningBulls, _activeOrderTracker.Bulls,
                    (_activeOrderTracker.Bulls > 0) ? (double)_activeOrderTracker.WinningBulls / _activeOrderTracker.Bulls : 0));
                Print(string.Format("{0} of {1} bears successful({2})", _activeOrderTracker.WinningBears, _activeOrderTracker.Bears,
                    (_activeOrderTracker.Bears > 0) ? (double)_activeOrderTracker.WinningBears / _activeOrderTracker.Bears : 0));
                Print(string.Format("{0} of {1} all successful({2})", _activeOrderTracker.WinningBears + _activeOrderTracker.WinningBulls, _activeOrderTracker.Bears + _activeOrderTracker.Bulls,
                    (_activeOrderTracker.Bears + _activeOrderTracker.Bulls > 0) ? (double)(_activeOrderTracker.WinningBears + _activeOrderTracker.WinningBulls) / (_activeOrderTracker.Bears + _activeOrderTracker.Bulls) : 0));


                 //if (DateTime.Parse(Time.ToString()).Minute > 21)
                  //  return;


               


                var isBull = IsBull();


                var isBear = IsBear();


                if (isBull || isBear)
                {
                    var expiryTime = barTime.AddHours(1);

                    var
                        order = new MoveGenericActiveOrder
                        {
                            Id = Guid.NewGuid(),
                            Time = barTime,
                            ExpiryHour = expiryTime.Hour,
                            ExpiryDay = expiryTime.Day,
                            EnteredAt = Close[0],
                            StrikeWidth = _strikeWidth
                        };


                    if (isBull)
                    {
                        order.IsLong = true;
                        order.ExitAt = Close[0] + (Math.Abs(_strikeWidth));
                        order.SettleAT = Close[0] + (Math.Abs(_strikeWidth*.25));
                        SendNotification(order);
                    }


                    else
                    {
                        order.IsLong = false;
                        order.ExitAt = Close[0] - (Math.Abs(_strikeWidth));
                        order.SettleAT = Close[0] - (Math.Abs(_strikeWidth*.25));
                        SendNotification(order);
                    }

                    _activeOrderTracker.AddOrder(order);
                }
            }
            catch (Exception e)
            {
                Print("error found:" + e.Message + " " + e.Source + " " + e.StackTrace);
                //Log("error found:" + e.Message + " " + e.Source + " " + e.StackTrace, LogLevel.Error);
            }
        }

        private bool IsBear()


        {
            if (!HasEnoughVoltility())
                return false;

            return IsBearCrossOver(0)  && Slope(StochasticsFunc().K, 1, 0) < 0;
        }

        private bool IsBearCrossOver(int barsAgo)
        {
            return StochasticsFunc().D[barsAgo] > StochasticsFunc().K[barsAgo] &&
                   StochasticsFunc().D[barsAgo + 1] < StochasticsFunc().K[barsAgo + 11];
        }

        private bool IsBull()
        {

            

            if (!HasEnoughVoltility())
                return false;

            return IsBullCrossOver(0)  && Slope(StochasticsFunc().K, 1, 0) > 0;
        }

        private bool IsBullCrossOver(int barsAgo)
        {
            return StochasticsFunc().D[barsAgo] < StochasticsFunc().K[barsAgo] &&
                   StochasticsFunc().D[barsAgo + 1] > StochasticsFunc().K[barsAgo + 1];
        }


        private Stochastics StochasticsFunc()
        {
            return Stochastics(3, 7, 3);
        }


        private bool HasEnoughVoltility()
        {
            if (CurrentBar < TrendStrength)
            {
                return false;
            }

            var vals = Enumerable.Range(0, TrendStrength).Select(z => High[z] - Low[z]).ToList();
            var avg = vals.Average();
            var stddev = Math.Sqrt(vals.Average(v => Math.Pow(v - avg, 2)));

            return stddev > _strikeWidth;
        }

        private void SendNotification(MoveGenericActiveOrder order)
        {
            var instrumentName = Instrument.ToString().Replace("Default", "").Replace(" ", "").Replace("$", "");

            var mailSubject = string.Format("KC-SIGNAL-{0}:  {1} @  {2}", instrumentName,
                (order.IsLong) ? "BULL" : "BEAR",
                Close[0]);
            var mailContentTemplate = @"A {0} signal was observed in '{1}' at {2} at a closing price of {3}.
Exit at {4}
Strike Width: {5}";
            var mailContent = string.Format(mailContentTemplate, (order.IsLong) ? "BULL" : "BEAR", Instrument, Time,
                Close[0], order.ExitAt, order.StrikeWidth);

            if (Historical)
                return;
            SendMail("hoskinsken@gmail.com", "hoskinsken@gmail.com", mailSubject, mailContent);
        }

  
    }
}