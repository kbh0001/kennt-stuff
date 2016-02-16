using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NinjaTrader.Strategy;

namespace NinjaTrader.Custom.Strategy
{
    public class MoveActiveOrderTracker
    {
        private readonly SortedList<Guid, MoveGenericActiveOrder> _activerOrders =
            new SortedList<Guid, MoveGenericActiveOrder>();

        private readonly Dictionary<string, StratStats> _strats = new Dictionary<string, StratStats>();


        private Action<string> _printFunction;
        private double _strikeWidth;


        public Action<string> PrintFunction
        {
            get { return _printFunction; }
            set { _printFunction = value; }
        }

        public double Cash
        {
            get { return _strats.Sum(outcome => outcome.Value.Cash); }
        }

        public void Print(string message)
        {
            if (_printFunction != null)
                _printFunction(message);
        }

        public void AddOrder(MoveGenericActiveOrder order)
        {
            var stratName = GetStratName(order);

            if (!_strats.ContainsKey(stratName))
            {
                _strats.Add(stratName, new StratStats());
            }

            _strats[stratName].Count++;

            _activerOrders.Add(order.Id, order);
        }

        private string GetStratName(MoveGenericActiveOrder order)
        {
            return order.ExitStrategy.GetType().Name;
        }

        public string GetStatusReport()
        {
      
            var _db = new StringBuilder();

            var totalAttempts = 0;
            var totalSuccess = 0;

            foreach (var stat in _strats)
            {
                totalAttempts += stat.Value.Count;
                totalSuccess += stat.Value.SuccessfulExits;
                totalSuccess += stat.Value.SuccessfulSettles;
        
                var successRate = 1.0*(stat.Value.SuccessfulExits + stat.Value.SuccessfulSettles)/stat.Value.Count;
                _db.AppendLine(stat.Key);
                _db.AppendLine(string.Format("Attempts : {0} ", stat.Value.Count));
                _db.AppendLine(string.Format("Success Ratio : {0} ", successRate));
                _db.AppendLine(string.Format("Cash: {0} ", stat.Value.Cash));

            }

            var totalSuccessRation = 1.0*totalSuccess/totalAttempts;
            _db.AppendLine("Summary");
            _db.AppendLine(string.Format("Attempts : {0} ", totalAttempts));
            _db.AppendLine(string.Format("Success Ratio : {0} ", totalSuccessRation));
            _db.AppendLine(string.Format("Cash: {0} ", this.Cash));

            return _db.ToString();
        }

        public void HandleCurrentOrders(DateTime now, double open, double close, double high, double low)
        {
            if (!_activerOrders.Any())
                return;


            var successfulExits =
                _activerOrders.Values.Where(z => z.ExitStrategy.ExitSuccessfull(z, now, open, close, high, low))
                    .ToList();
            foreach (var success in successfulExits)
            {
                var stratName = GetStratName(success);
                _strats[stratName].SuccessfulExits++;
                _strats[stratName].Cash += success.ExitStrategy.CashOnSuccesfulExit;
                _activerOrders.Remove(success.Id);
            }


            var failures =
                _activerOrders.Values.Where(z => z.ExitStrategy.ExitFailed(z, now, open, close, high, low)).ToList();
            foreach (var success in failures)
            {
                var stratName = GetStratName(success);
                _strats[stratName].Cash += success.ExitStrategy.CashOnFailedExit;
                _strats[stratName].FailedExits++;
                _activerOrders.Remove(success.Id);
            }


            if (now.Minute == 00)
            {
                var closingOrders =
                    _activerOrders.Values.Where(
                        z => z.ExpiryDay == now.Day && z.ExpiryHour == now.Hour - 1)
                        .ToList();

                foreach (var candidate in closingOrders)
                {
                    var stratName = GetStratName(candidate);

                    if (candidate.ExitStrategy.SettlesSuccessFull(candidate, open))
                    {
                        _strats[stratName].SuccessfulSettles++;
                        _strats[stratName].Cash += candidate.ExitStrategy.CashOnSuccessfulSettle;
                    }
                    else
                    {
                        _strats[stratName].FailedSettles++;
                        _strats[stratName].Cash += candidate.ExitStrategy.CashOnFailedSettle;
                    }
                    _activerOrders.Remove(candidate.Id);
                }
            }
        }

        private class StratStats
        {
            public Double Cash;
            public int Count;
            public int FailedExits;
            public int FailedSettles;
            public int SuccessfulExits;
            public int SuccessfulSettles;
        }
    }
}