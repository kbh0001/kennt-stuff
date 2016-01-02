// This namespace holds all strategies and is required. Do not change it.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using KenNinja;

namespace NinjaTrader.Custom.Strategy
{
    /// <summary>
    /// Get Some Data
    /// </summary>
    [Description("Get Some Data")]
    public class KenCandleStickStrategy : NinjaTrader.Strategy.Strategy
    {
        // User defined variables (add any user defined variables below)

        private static readonly List<Kp> KpsToUse;
        private static int tradeId = 0;
        protected IDbConnection DbConn;
        private SortedList<System.Guid, ActiveOrder> activerOrders;
        private int myInput0 = 1; // Default setting for MyInput0
        private int _bulls;
        private int _winningBulls;


        //Configure the allowed patterns that are significant order by performance.
        static KenCandleStickStrategy()
        {
            var list = new[]
            {
                109,
                111, 103,
                105,
                -107,
                -202,
                -102,
                102,
                106,
                -112,
                -104,
                -103,
                104
            };
            var validValues = Enum.GetValues(typeof (Kp)).Cast<Kp>().Select(z => z.ToInt());
            KpsToUse = list.Where(validValues.Contains).Cast<Kp>().ToList();
        }


        protected override void Initialize()
        {
            ClearOutputWindow();
            CalculateOnBarClose = true; // only on bar close( this is a candle stick strategy)
            activerOrders = new SortedList<System.Guid, ActiveOrder>();
            _bulls = 0;
            _winningBulls = 0;
        }


        protected override void OnBarUpdate()
        {
            HandleCurrentOrders();
            Print(string.Format("{0} of {1} bulls successful", _winningBulls, _bulls));


            double candlestick = 0;

            var barTime = DateTime.Parse(Time.ToString());

            if (barTime.Minute == 45)
            {
                //no orders in final bar
                return;
            }

            //Is there any sentiment found
            foreach (var dood in KpsToUse)
            {
                candlestick = KenCandleStickPattern(dood, 4)[0];
                if (IsBullishSentiment(candlestick) || IsBearishSentiment(candlestick))
                {
                    break;
                }
                candlestick = 0;
            }


            var expiryTime = barTime.AddHours(1);

            var
                order = new ActiveOrder
                {
                    Id = Guid.NewGuid(),
                    Time = barTime,
                    ExpiryHour = expiryTime.Hour,
                    ExpiryDay = expiryTime.Day,
                    EnteredAt = Close[0]
                };


            if (IsBullishSentiment(candlestick))
            {
                order.IsLong = true;
                order.ExitAt = Close[0] + .02;
                activerOrders.Add(order.Id, order);
                _bulls++;
            }

            if (IsBearishSentiment(candlestick))
            {
            }
        }

        private void HandleCurrentOrders()
        {
            if (activerOrders.Any())
            {
                var currentNow = DateTime.Parse(Time.ToString());

                var successfulBulls = activerOrders.Values.Where(z => z.IsLong && this.High[0] > z.ExitAt).ToList();
                foreach (var success in successfulBulls)
                {
                    activerOrders.Remove(success.Id);
                    _winningBulls++;

                }

                if (currentNow.Minute == 45)
                {
                    var closingOrders =
                        activerOrders.Values.Where(z => z.ExpiryDay == currentNow.Day && z.ExpiryHour == currentNow.Hour)
                            .ToList();

                    foreach (var candidate in closingOrders)
                    {
                        if (candidate.EnteredAt < this.Close[0])
                        {
                            _winningBulls++;
                        }
                        activerOrders.Remove(candidate.Id);
                    }
                }
            }
        }


        private static bool IsBearishSentiment(double candleStick)
        {
            return candleStick < -99;
        }

        private static bool IsBullishSentiment(double candleStick)
        {
            return candleStick > 99;
        }

        private class ActiveOrder
        {
            public Guid Id { get; set; }
            public DateTime Time { get; set; }
            public int ExpiryHour { get; set; }
            public int ExpiryDay { get; set; }
            public bool IsLong { get; set; }
            public double EnteredAt { get; set; }
            public double ExitAt { get; set; }
        }
    }
}