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
        private Action<string> _printFunction;
        


       


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

        public Action<string> PrintFunction
        {
            get { return _printFunction; }
            set { _printFunction = value; }
        }

        public void Print(string message)
        {
            if (_printFunction != null)
                _printFunction(message);
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
           
            
            var successfulBulls = _activerOrders.Values.Where(z => z.IsLong && z.ExitStrategy.ExitSuccessfull(z, now, open, close, high, low)).ToList();
            foreach (var success in successfulBulls)
            {
                _activerOrders.Remove(success.Id);
                _winningBulls++;
            }

            


            var successfulBears = _activerOrders.Values.Where(z => !z.IsLong && z.ExitStrategy.ExitSuccessfull(z, now, open, close, high, low)).ToList();
            foreach (var success in successfulBears)
            {
                _activerOrders.Remove(success.Id);
                _winningBears++;
            }


            var failures = _activerOrders.Values.Where(z =>z.ExitStrategy.ExitFailed(z, now, open, close, high, low)).ToList();
            foreach (var success in failures)
            {
                _activerOrders.Remove(success.Id);
            }
            


            if (now.Minute == 00)
            {
                var closingOrders =
                    _activerOrders.Values.Where(
                        z => z.ExpiryDay == now.Day && z.ExpiryHour == now.Hour-1)
                        .ToList();

                foreach (var candidate in closingOrders)
                {
                    
                    if (candidate.IsLong && candidate.SuccessFullySettlesAt < open)
                    {
                        _winningBulls++;
                    }
                    if (!candidate.IsLong && candidate.SuccessFullySettlesAt > open)
                    {
                        _winningBears++;
                    }
                    _activerOrders.Remove(candidate.Id);
                }
            }

        }
    }
}