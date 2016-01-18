// This namespace holds all strategies and is required. Do not change it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using KenNinja;
using NinjaTrader.Cbi;
using NinjaTrader.Indicator;
using NinjaTrader.Strategy;

namespace NinjaTrader.Custom.Strategy
{
    /// <summary>
    /// Get Some Data
    /// </summary>
    [Description("Get Some Data")]
    public class StocCross : NinjaTrader.Strategy.Strategy
    {
        // User defined variables (add any user defined variables below)

        private const int TrendStrength = 4;
        private static readonly List<Kp> KpsToUse;
        private static int tradeId = 0;
        private SortedList<Guid, ActiveOrder> _activerOrders;
        private int _bears;
        private int _bulls;
        private double _strikeWidth;
        private int _winningBears;
        private int _winningBulls;


        //Configure the allowed patterns that are significant order by performance.
        static StocCross()
        {
        }


        protected override void Initialize()
        {

            
            var _binaryWidths = new SortedList<string, double>
            {
                {"$EURUSD", .0004},
                {"$GBPUSD", .0010},
                {"$USDJPY", .04},
                {"$AUDUSD", .0005},
                {"$USDCAD", .0010},
                {"$EURJPY", .1}
            };

            //var  _spreadWidths = new SortedList<string, double> { { "$EURUSD", .0004 }, { "$GBPUSD", .001 }, { "$USDJPY", 1.0 }, { "$AUDUSD", .01 }, { "$USDCAD", .01 }, { "$EURJPY", 1.0 } };


            var _stikeWidths = _binaryWidths;
            var instrument = Instrument.ToString().ToUpper().Replace("DEFAULT", "").Replace(" ", "");
            Print(instrument);
            if (_stikeWidths.ContainsKey(instrument))
            {
                _strikeWidth = _stikeWidths[instrument];
            }
            else
            {
                Print("Fail");
            }


            Log(string.Format("Starting for KenCandleStickStrategy {0}", Instrument), LogLevel.Information);
            // ClearOutputWindow();
            CalculateOnBarClose = true; //only on bar close
            _activerOrders = new SortedList<Guid, ActiveOrder>();
            _bulls = 0;
            _winningBulls = 0;
            _bears = 0;
            _winningBears = 0;
        }


        protected override void OnBarUpdate()
        {
            try
            {
                HandleCurrentOrders();

                
                Print("");

                Print(string.Format("{0} of {1} bulls successful({2})", _winningBulls, _bulls,
                    (_bulls > 0) ? (double) _winningBulls/_bulls : 0));
                Print(string.Format("{0} of {1} bears successful({2})", _winningBears, _bears,
                    (_bears > 0) ? (double) _winningBears/_bears : 0));
                Print(string.Format("{0} of {1} all successful({2})", _winningBears + _winningBulls, _bears + _bulls,
                    (_bears + _bulls > 0) ? (double) (_winningBears + _winningBulls)/(_bears + _bulls) : 0));
                

                double candlestick = 0;

                var barTime = DateTime.Parse(Time.ToString());

                //Make Sure there is enough movement to matter

                if (!HasEnoughVoltility())
                    return;


                var isBull = CrossAbove(StochasticsFunc().D, StochasticsFunc().K, 8) &&
                             StochasticsFunc().D[0] < 20;
                var isBear = CrossBelow(StochasticsFunc().D, StochasticsFunc().K, 8) &&
                             StochasticsFunc().D[0] > 80;


                if (isBull || isBear)
                {
                    var expiryTime = barTime.AddHours(1);

                    var
                        order = new ActiveOrder
                        {
                            Id = Guid.NewGuid(),
                            Time = barTime,
                            ExpiryHour = expiryTime.Hour,
                            ExpiryDay = expiryTime.Day,
                            EnteredAt = Close[0],
                            StrikeWidth = _strikeWidth
                        };



                    if (isBull && IsBullPrevailingTrend())
                    {

                        order.IsLong = true;
                        order.ExitAt = Close[0] + (Math.Abs(_strikeWidth*.5));
                        order.SettleAT = Close[0] + (Math.Abs(_strikeWidth * .25));
                        _activerOrders.Add(order.Id, order);
                        _bulls++;

                        SendNotification(candlestick, order);
                    }


                    if (isBear && IsBearPrevailingTrend())
                    {
                        order.IsLong = false;
                        order.ExitAt = Close[0] - (Math.Abs(_strikeWidth*.5));
                        order.SettleAT = Close[0] - (Math.Abs(_strikeWidth * .25));

                        _activerOrders.Add(order.Id, order);
                        _bears++;
                        SendNotification(candlestick, order);
                    }
                     
                   
                }
            }
            catch (Exception e)
            {
                Print("error found:" + e.Message + " " + e.Source + " " + e.StackTrace);
                //Log("error found:" + e.Message + " " + e.Source + " " + e.StackTrace, LogLevel.Error);
            }
        }

        private Stochastics StochasticsFunc()
        {
            return Stochastics(3, 7, 3);
        }

        private bool IsBullPrevailingTrend()
        {
            return true;
            return Slope(SMA(20), 8, 0) > 0;

        }

        private bool IsBearPrevailingTrend()
        {
            return true;
            return Slope(SMA(20), 8, 0) < 0;

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

        private void SendNotification(double candlestick, ActiveOrder order)
        {
            


            var instrumentName = Instrument.ToString().Replace("Default", "").Replace(" ", "").Replace("$", "");

            var mailSubject = string.Format("KC-SIGNAL-{0}:  {1} @  {2}", instrumentName,
                (order.IsLong) ? "BULL" : "BEAR",
                Close[0]);
            var mailContentTemplate = @"A {0} signal was observed in '{1}' at {2} at a closing price of {3}.
Exit at {4}
Strike Width: {4}";
            var mailContent = string.Format(mailContentTemplate, (order.IsLong) ? "BULL" : "BEAR", Instrument, Time,
                Close[0], order.ExitAt, order.StrikeWidth);
			
			if (Historical)
                return;
            SendMail("hoskinsken@gmail.com", "hoskinsken@gmail.com", mailSubject, mailContent);
        }

        private void HandleCurrentOrders()
        {
            if (!_activerOrders.Any())
                return;

            
                var currentNow = DateTime.Parse(Time.ToString());
                
                var successfulBulls = _activerOrders.Values.Where(z => z.IsLong && High[0] >= z.ExitAt).ToList();
                foreach (var success in successfulBulls)
                {
                    _activerOrders.Remove(success.Id);
                    _winningBulls++;
                }


                var successfulBears = _activerOrders.Values.Where(z => !z.IsLong && Low[0] <= z.ExitAt).ToList();
                foreach (var success in successfulBears)
                {
                    _activerOrders.Remove(success.Id);
                    _winningBears++;
                }
                

                if (currentNow.Minute == 00)
                {
                    var closingOrders =
                        _activerOrders.Values.Where(
                            z => z.ExpiryDay == currentNow.Day && z.ExpiryHour == currentNow.Hour)
                            .ToList();

                    foreach (var candidate in closingOrders)
                    {
                        if (candidate.IsLong && candidate.SettleAT < Open[0])
                        {
                            _winningBulls++;
                        }
                        if (!candidate.IsLong && candidate.SettleAT > Open[0])
                        {
                            _winningBears++;
                       }
                        _activerOrders.Remove(candidate.Id);
                    }
                }
            
        }


        private class ActiveOrder
        {
            public double StrikeWidth { get; set; }
            public Guid Id { get; set; }
            public DateTime Time { get; set; }
            public int ExpiryHour { get; set; }
            public int ExpiryDay { get; set; }
            public bool IsLong { get; set; }
            public double EnteredAt { get; set; }
            public double ExitAt { get; set; }
            public double SettleAT { get; set; }
        }
    }
}