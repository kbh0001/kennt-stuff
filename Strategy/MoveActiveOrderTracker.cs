using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using NinjaTrader.Indicator;

namespace NinjaTrader.Custom.Strategy
{
    public class MoveActiveOrderTracker
    {
        private readonly SortedList<Guid, MoveGenericActiveOrder> _activerOrders = new SortedList<Guid, MoveGenericActiveOrder>();
        private int _bears;
        private int _bulls;
        private double _strikeWidth;
        private int _winningBears;
        private int _winningBulls;


        public int Bears
        {
            get { return _bears; }
        }

        public int Bulls
        {
            get { return _bulls; }
        }

        public int WinningBears
        {
            get { return _winningBears; }
        }

        public int WinningBulls
        {
            get { return _winningBulls; }
        }

        public void AddOrder(MoveGenericActiveOrder order)
        {
            if (order.IsLong)
                _bulls++;
            else
            {
                _bears++;
            }

            _activerOrders.Add(order.Id, order);
        }

        public void HandleCurrentOrders(DateTime now, double open, double close, double high, double low)
        {
            if (!_activerOrders.Any())
                return;
           

            var successfulBulls = _activerOrders.Values.Where(z => z.IsLong && high >= z.ExitAt).ToList();
            foreach (var success in successfulBulls)
            {
                _activerOrders.Remove(success.Id);
                _winningBulls++;
            }


            var successfulBears = _activerOrders.Values.Where(z => !z.IsLong && low <= z.ExitAt).ToList();
            foreach (var success in successfulBears)
            {
                _activerOrders.Remove(success.Id);
                _winningBears++;
            }


            if (now.Minute == 00)
            {
                var closingOrders =
                    _activerOrders.Values.Where(
                        z => z.ExpiryDay == now.Day && z.ExpiryHour == now.Hour)
                        .ToList();

                foreach (var candidate in closingOrders)
                {
                    if (candidate.IsLong && candidate.SettleAT < open)
                    {
                        _winningBulls++;
                    }
                    if (!candidate.IsLong && candidate.SettleAT > open)
                    {
                        _winningBears++;
                    }
                    _activerOrders.Remove(candidate.Id);
                }
            }

        }
    }
}