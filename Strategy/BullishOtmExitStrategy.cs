using System;

namespace NinjaTrader.Custom.Strategy
{
    public class BullishOtmExitStrategy : MoveExitStrategyBase
    {
        private readonly double _exitAt;
        private readonly double _settlesAt;

        public BullishOtmExitStrategy(double exitAmount, double settleAmount)
        {
            _exitAt = exitAmount;
            _settlesAt = settleAmount;
        }


        public override string ExitStategyDescr
        {
            get { return "Exit at: " + _exitAt; }
        }

        public override bool ExitSuccessfull(MoveGenericActiveOrder order, DateTime now, double open, double close,
            double high, double low)
        {
            if (high >= _exitAt)
                return true;

            return false;
        }

        public override bool ExitFailed(MoveGenericActiveOrder order, DateTime now, double open, double close,
            double high, double low)
        {
            return false;
        }

        public override bool SettlesSuccessFull(MoveGenericActiveOrder order, double close)
        {
            return close > _settlesAt;
        }

        public override double CashOnSuccesfulExit
        {
            get { return 20; }
        }

        public override double CashOnSuccessfulSettle
        {
            get { return 65; }
        }

        public override double CashOnFailedSettle
        {
            get { return -35; }
        }

        public override double CashOnFailedExit
        {
            get { throw new NotImplementedException(); }
        }
    }
}